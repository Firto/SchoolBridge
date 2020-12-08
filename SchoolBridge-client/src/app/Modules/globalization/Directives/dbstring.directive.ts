import { Directive, ElementRef, Input, ViewContainerRef, OnInit, Renderer2, HostListener, OnDestroy, Optional, Inject, ChangeDetectorRef, AfterViewInit } from '@angular/core'
import { Subscription, Observable, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { OnUnsubscribe } from 'src/app/Services/super.controller';
import { GlobalizationService } from '../Services/globalization.service';
import { GlobalizationEditService } from '../Services/globalization-edit.service';
import { GlobalizationStringService } from '../Services/globalization-string.service';
import { ParentComponent } from 'src/app/Services/parent.component';
import { MdGlobalizationService } from '../Services/md-globalization.service';

class DbString {
  constructor(
    public readonly name: string,
    public readonly elementRef: ElementRef,
    public readonly renderer: Renderer2,
    public readonly type: "html" | "attr",
    public readonly obs: Observable<string>,
    public readonly attr: string = "") {

    this.obs.subscribe(x => {
      if (this.type !== 'html')
        this.renderer.setAttribute(this.elementRef.nativeElement, this.attr, x);
      else this.elementRef.nativeElement.innerHTML = x;
    });
  }
}

@Directive({
  selector: '[dbstring]'
})
export class DbStringDirective extends OnUnsubscribe implements AfterViewInit {
  @Input('attrString') arg: { str: string, arg: string };
  @Input('dbPrefix') prefix: boolean = true;

  private _strings: DbString[] = [];

  constructor(private _elementRef: ElementRef,
    private _renderer: Renderer2,
    private _mdGbService: MdGlobalizationService,
    private _gbsService: GlobalizationStringService,
    private _gbeService: GlobalizationEditService) {
    super();
  }

  getUsedStrings(): string[] {
    return this._strings.map(x => x.name);
  }

  changeState(s: Boolean) {
    if (s)
      this._renderer.addClass(this._elementRef.nativeElement, 'editing');
    else this._renderer.removeClass(this._elementRef.nativeElement, 'editing');
  }

  ngAfterViewInit() {
    this._gbeService.stateObs.pipe(takeUntil(this._destroy)).subscribe(x => {
      this.changeState(x);
    });

    if (this.arg)
      this._strings.push(
        new DbString(
          this._mdGbService.getFullStringName(this.arg.str, this.prefix),
          this._elementRef,
          this._renderer,
          "attr",
          this._mdGbService.useString(this.arg.str, this.prefix),
          this.arg.arg)
      );

    if (this._elementRef.nativeElement.innerHTML) {
      let key = (<string>this._elementRef.nativeElement.innerHTML).replace(' ', "");
      this._strings.push(
        new DbString(
          this._mdGbService.getFullStringName(key, this.prefix),
          this._elementRef,
          this._renderer,
          "html",
          this._mdGbService.useString(key, this.prefix))
      );
    }
  }

  public getString(name: string): string {

    return this._gbsService.getLoadedStringSave(name, "");
  }

  public constStrings(): string[] {
    const mp = this._strings.map(x => x.name);
    return this._mdGbService.constStrings.filter(x => !mp.includes(x));
  }

  @HostListener('contextmenu', ['$event']) onClick(ev: MouseEvent) {
    if (ev.button != 2 || !this._gbeService.state) return true;
    ev.preventDefault();
    this._gbeService.changeEditing(this);
    return false;
  }
}
