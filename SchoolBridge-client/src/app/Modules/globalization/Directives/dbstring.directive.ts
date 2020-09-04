import { Directive, ElementRef, Input, ViewContainerRef, OnInit, Renderer2, HostListener, OnDestroy, Optional } from '@angular/core'
import { Subscription, Observable, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { OnUnsubscribe } from 'src/app/Services/super.controller';
import { GlobalizationService } from '../Services/globalization.service';
import { GlobalizationEditService } from '../Services/globalization-edit.service';

class DbString{
    private _obs: Observable<string>;  

    public set obs(val: Observable<string>) {
      this._obs = val;
      if (this._obs) 
        this._obs.subscribe(x => {
          if (this._type !== 'html')
            this._renderer.setAttribute(this._elementRef.nativeElement, this._attr, x);
          else this._elementRef.nativeElement.innerHTML = x;
        });
    }

    public get key(): string{
      return this._key;
    }
    public get type(): string{
      return this._type;
    }
    public get obs(): Observable<string>{
      return this._obs;
    }
    public get attr(): string{
      return this._attr;
    }

    constructor ( private _elementRef: ElementRef,
                  private _renderer: Renderer2,
                  private _key: string,
                  private _type: "html" | "attr",
                  private _attr: string = ""){
      
    }
}

@Directive({
  selector: '[dbstring]',
})
export class DbStringDirective extends OnUnsubscribe implements OnInit {
  @Input('attrString') arg: {str: string, arg: string};
  @Input('dbPrefix') prefix: boolean = true;

  private _strings: Record<string, DbString> = {};
  private _componentInfo: {prefix: string, constStrings: string[]};

  get componentInfo(): {prefix: string, constStrings: string[]}{
    return this._componentInfo;
  }

  constructor(private _elementRef: ElementRef,
                private _renderer: Renderer2,
                private _viewContainerRef: ViewContainerRef,
                private _gbService: GlobalizationService,
                private _gbeService: GlobalizationEditService) {
    super();
    this._componentInfo = this._gbService.getComponent((<any>this._viewContainerRef)._hostView[8].__proto__.constructor.__proto__.name)
    if (!this._componentInfo) 
      this._componentInfo = {prefix: 'cm', constStrings: []};
  }

  getUsedStrings(): string[]{
    return Object.keys(this._strings);
  }

  changeState(s: Boolean){
    if (s)
        this._renderer.addClass(this._elementRef.nativeElement, 'editing');
    else this._renderer.removeClass(this._elementRef.nativeElement, 'editing');
  }

  ngOnInit(){
    this._gbeService.stateObs.pipe(takeUntil(this._destroy)).subscribe(x => {
      this.changeState(x);
    });

    if (this.arg){
      if (this.prefix === true)
        this.arg.str = this._componentInfo.prefix + '-' + this.arg.str;
      this._strings[this.arg.str] = new DbString(this._elementRef, this._renderer, this.arg.str, "attr", this.arg.arg);
      //this._renderer.setAttribute(this._elementRef.nativeElement, this.arg.arg, DbStringDirective.__noneStringDef);
    }
    
    if (this._elementRef.nativeElement.innerHTML){
      let key = this._elementRef.nativeElement.innerHTML;
      if (this.prefix === true) 
        key = this._componentInfo.prefix + '-' + key;
      this._strings[key] =  new DbString(this._elementRef, this._renderer, key, "html");
    }
    
    

    const som = this._gbService.gbStringService.getStringsObs(Object.keys(this._strings))
    //console.log(som);
    Object.keys(som).forEach(x => {
      this._strings[x].obs = som[x];
    })
  }

  public getString(name: string): string {
    return this._gbService.gbStringService.getLoadedStringSave(name, "");
  }

  public constStrings(): string[]{
      return this._componentInfo.constStrings.filter(x => !Object.keys(this._strings).includes(x));
  }
  
  @HostListener('contextmenu', ['$event']) onClick(ev: MouseEvent) {
    if (ev.button != 2 || !this._gbeService.state) return true;
    ev.preventDefault();
    this._gbeService.changeEditing(this);
    return false;
  }
}