import {Pipe, PipeTransform, NgZone, ChangeDetectorRef, OnDestroy, Directive, OnInit, ElementRef, Renderer2, HostListener, OnChanges, AfterContentInit, SimpleChanges, Input} from "@angular/core";
import { Observable, Subscription } from 'rxjs';
import { OnUnsubscribe } from 'src/app/Services/super.controller';
import { GlobalizationEditService } from '../globalization/Services/globalization-edit.service';
import { GlobalizationStringService } from '../globalization/Services/globalization-string.service';
import { filter, mergeMap, takeUntil } from 'rxjs/operators';
import { TimeAgoDirectiveService } from './time-ago-directive.service';

@Directive({
	selector: '[timeAgo]'
})
export class TimeAgoDirective extends OnUnsubscribe implements OnChanges {
	private _curStr: string = null;
	private _subs: Subscription = null;

	@Input('timeAgo') public date: string;

	constructor(private _tmgService: TimeAgoDirectiveService,
				private _elementRef: ElementRef,
				private _gbsService: GlobalizationStringService,
				private _gbeService: GlobalizationEditService,
				private _renderer: Renderer2) {
		super();

		this._gbeService.stateObs.pipe(takeUntil(this._destroy)).subscribe(x => {
			this.changeState(x);
	  	});
	}

	getUsedStrings(): string[]{
		return [this._curStr];
	}
	
	changeState(s: Boolean){
		if (s)
			this._renderer.addClass(this._elementRef.nativeElement, 'editing');
		else this._renderer.removeClass(this._elementRef.nativeElement, 'editing');
	}
	
	ngOnChanges(changes: SimpleChanges){
		if (!this.date) return;
		if (this._subs)
			this._subs.unsubscribe();

		this._subs = this._tmgService.getStringObservable(new Date(this.date)).pipe(
			takeUntil(this._destroy),
			filter(x => x && x.length > 0 ),
			mergeMap(x => {
				this._curStr = this._gbsService.getConstStringName(x);
				return this._gbsService.convertString(x);
			})
		).subscribe(x => 
			this._renderer.setProperty(this._elementRef.nativeElement, 'innerHTML', x)
		);
	}
	
	public getString(name: string): string {
		return this._gbsService.getLoadedStringSave(name, "");
	}
	
	public constStrings(): string[]{
		  return this._tmgService.getConstStrings().filter(v => v !== this._curStr);
	}
	  
	@HostListener('contextmenu', ['$event']) onClick(ev: MouseEvent) {
		if (ev.button != 2 || !this._gbeService.state) return true;
		ev.preventDefault();
		this._gbeService.changeEditing(this);
		return false;
	}
}