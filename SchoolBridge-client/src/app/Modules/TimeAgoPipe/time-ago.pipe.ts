import {Pipe, PipeTransform, NgZone, ChangeDetectorRef, OnDestroy} from "@angular/core";
import { TimeAgoPipeService } from './time-ago.service';
import { Observable } from 'rxjs';
@Pipe({
	name:'timeAgo',
	pure:true
})
export class TimeAgoPipe implements PipeTransform, OnDestroy {

	constructor(private _tmgService: TimeAgoPipeService) {}

	transform(value:string | Date): Observable<string>{
		return this._tmgService.getStringObservable(new Date(value));
	}
	
	ngOnDestroy(): void {
		
	}
}