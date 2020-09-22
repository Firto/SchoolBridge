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
  public get type(): string {
    return this._type;
  }
  public get obs(): Observable<string> {
    return this._obs;
  }
  public get attr(): string {
    return this._attr;
  }

  constructor(private _elementRef: ElementRef,
    private _renderer: Renderer2,
    private _type: "html" | "attr",
    private _obs: Observable<string>,
    private _changeDetectorRef: ChangeDetectorRef,
    private _attr: string = "") {
    this._obs.subscribe(x => {
      if (this._type !== 'html')
        this._renderer.setAttribute(this._elementRef.nativeElement, this._attr, x);
      else this._elementRef.nativeElement.innerHTML = x;
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
  private _componentInfo: { prefix: string, constStrings: string[] };

  get componentInfo(): { prefix: string, constStrings: string[] } {
    return this._componentInfo;
  }

  constructor(private _elementRef: ElementRef,
    private _renderer: Renderer2,
    private _mdGbService: MdGlobalizationService,
    private _gbsService: GlobalizationStringService,
    private _gbeService: GlobalizationEditService,
    private _changeDetectorRef: ChangeDetectorRef) {
    super();
  }

  getUsedStrings(): string[] {
    return Object.keys(this._strings);
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
          this._elementRef,
          this._renderer,
          "attr",
          this._mdGbService.useString(this.arg.str, this.prefix),
          this._changeDetectorRef,
          this.arg.arg)
      );

    if (this._elementRef.nativeElement.innerHTML) {
      let key = this._elementRef.nativeElement.innerHTML;
      this._strings.push(
        new DbString(
          this._elementRef,
          this._renderer,
          "html",
          this._mdGbService.useString(key, this.prefix),
          this._changeDetectorRef)
      );
    }
  }

  public getString(name: string): string {
    return this._gbsService.getLoadedStringSave(name, "");
  }

  public constStrings(): string[] {
    return [];
  }

  @HostListener('contextmenu', ['$event']) onClick(ev: MouseEvent) {
    if (ev.button != 2 || !this._gbeService.state) return true;
    ev.preventDefault();
    this._gbeService.changeEditing(this);
    return false;
  }
}