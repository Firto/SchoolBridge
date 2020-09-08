import { Directive, ElementRef, Input, ViewContainerRef, OnInit, Renderer2, HostListener, OnDestroy, Optional, Inject } from '@angular/core'
import { Subscription, Observable, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { OnUnsubscribe } from 'src/app/Services/super.controller';
import { GlobalizationService } from '../Services/globalization.service';
import { GlobalizationEditService } from '../Services/globalization-edit.service';
import { GlobalizationStringService } from '../Services/globalization-string.service';


@Directive({
  selector: '[constdbstring]',
})
export class ConstDbStringDirective {
  private _componentInfo: {prefix: string, constStrings: string[]};

  get componentInfo(): {prefix: string, constStrings: string[]}{
    return this._componentInfo;
  }

  constructor(private _elementRef: ElementRef,
                private _viewContainerRef: ViewContainerRef,
                private _gbService: GlobalizationService,
                private _gbsService: GlobalizationStringService,
                private _gbeService: GlobalizationEditService) {
    this._componentInfo = this._gbService.getComponent((<any>this._viewContainerRef)._hostView[8].__proto__.constructor.__proto__.name)
  }

  public getString(name: string): string {
    return this._gbsService.getLoadedStringSave(name, "");
  }
  
  public getUsedStrings(): string[]{
    return Object.keys(this._componentInfo.constStrings);
  }

  @HostListener('contextmenu', ['$event']) onClick(ev: MouseEvent) {
    if (ev.button != 2 || !this._gbeService.state) return true;
    ev.preventDefault();
    if (this._componentInfo)
        this._gbeService.changeEditing(this);
    return false;
  }
}