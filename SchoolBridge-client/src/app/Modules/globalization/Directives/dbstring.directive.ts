import { Directive, ElementRef, Input, ViewContainerRef, OnInit, Renderer2, HostListener, OnDestroy } from '@angular/core'
import { GlobalizationService } from '../services/globalization.service'
import { Subscription, Observable, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { OnUnsubscribe } from 'src/app/Services/super.controller';

@Directive({
  selector: '[dbstring]',
})
export class DbStringDirective extends OnUnsubscribe implements OnInit {
  private static __noneStringDef: string = "-none-";

  @Input('attrString') arg: {str: string, arg: string};
  @Input('dbPrefix') prefix: boolean = true;

  private _strings: Record<string, {type: string | 'html', value: string}> = {};

  private _defBackStringPrefix: string = 'cm';
  private _obs: Observable<Record<string, string>> = null;

  constructor(private _elementRef: ElementRef,
                private _renderer: Renderer2,
                private _viewContainerRef: ViewContainerRef,
                private _globalizationService: GlobalizationService) {
    super(); 
    if ((<any>this._viewContainerRef)._hostView[8]._defBackStringPrefix)
      this._defBackStringPrefix = (<any>this._viewContainerRef)._hostView[8]._defBackStringPrefix;

    /*document.addEventListener('contextmenu', function (e) {
      console.log(e);
    }, false);*/
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
    this._globalizationService.changeEditing.pipe(takeUntil(this._destroy)).subscribe(x => {
      this.changeState(x);
    });

    if (this.arg){
      if (this.prefix)
        this.arg.str = this._defBackStringPrefix + '-' + this.arg.str;
      this._strings[this.arg.str] = {type: this.arg.arg, value: ''};
      //this._renderer.setAttribute(this._elementRef.nativeElement, this.arg.arg, DbStringDirective.__noneStringDef);
    }
    
    if (this._elementRef.nativeElement.innerHTML){
      this._strings[this.prefix ? this._defBackStringPrefix + '-' + this._elementRef.nativeElement.innerHTML : this._elementRef.nativeElement.innerHTML] = {type: 'html', value: ''};
    }
    
    console.log(this._strings);

    this._obs = this._globalizationService.initLocalStrings(...Object.keys(this._strings));

    this._obs.subscribe(k => {
      this.setStrings(k);
      this.updateStrings();
    });

    if(this._globalizationService.isIssetDataForCurrentLanguage())
      this.setStrings(this._globalizationService.getLocalStrings(this.getUsedStrings()));
    this.updateStrings();
  }

  private setStrings(strings: Record<string, string>){
    for (const [key, value] of Object.entries(strings))
      this._strings[key].value = value;
  }

  private updateStrings(){
    for (const [key, value] of Object.entries(this._strings)){
      if (value.type !== 'html')
        this._renderer.setAttribute(this._elementRef.nativeElement, value.type, value.value.length == 0 ? DbStringDirective.__noneStringDef : value.value);
      else this._elementRef.nativeElement.innerHTML = value.value.length == 0 ? DbStringDirective.__noneStringDef : value.value;
    }
  }

  public getString(name: string): string {
    return this._strings[name].value;
  }

  @HostListener('contextmenu', ['$event']) onClick(ev: MouseEvent) {
    if (ev.button != 2) return true;
    ev.preventDefault();
    this._globalizationService.showStringEditWindow(this);
    return false;
  }
}