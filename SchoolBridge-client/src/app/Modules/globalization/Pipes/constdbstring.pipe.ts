import {Pipe, PipeTransform, NgZone, ChangeDetectorRef, OnDestroy, ElementRef, Renderer2, Directive, AfterViewInit, OnInit, HostListener} from "@angular/core";
import { Observable } from 'rxjs';
import { GlobalizationStringService } from '../Services/globalization-string.service';
import { mergeMap, takeUntil } from 'rxjs/operators';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';
import { GlobalizationEditService } from '../Services/globalization-edit.service';
import { OnUnsubscribe } from 'src/app/Services/super.controller';

@Directive({
    selector: '[constdbstring]',
})
export class ConstDbStringPipe extends OnUnsubscribe implements OnInit {
    private _str: string = null;

    constructor(private _elementRef: ElementRef,
                private _gbsService: GlobalizationStringService,
                private _gbeService: GlobalizationEditService,
                private _renderer: Renderer2) {
        super();

        this._gbeService.stateObs.pipe(takeUntil(this._destroy)).subscribe(x => {
            this.changeState(this._elementRef, x);
        });
    }

    public ngOnInit(){
        
    }

    public getUsedStrings(): string[]{
        return [this._str];
    }

    public getString(name: string): string {
        return this._gbsService.getLoadedStringSave(name, "");
      }
    
    public constStrings(): string[]{
        return [];
    }

    changeState(elem:ElementRef, s: Boolean){
        if (s)
            this._renderer.addClass(elem.nativeElement, 'editing');
        else this._renderer.removeClass(elem.nativeElement, 'editing');
    }

    @HostListener('contextmenu', ['$event']) onClick(ev: MouseEvent) {
        if (ev.button != 2 || !this._gbeService.state) return true;
        ev.preventDefault();
        this._gbeService.changeEditing(this);
        return false;
    }

    @HostListener('change', ['$event']) onChange(ev: Event) {
        console.log(ev);
        return true;
    }
}