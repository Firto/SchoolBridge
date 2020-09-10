import { Injectable, Injector } from '@angular/core';
import { Observable, interval, Subject, BehaviorSubject, of } from 'rxjs';
import { map, takeUntil, tap, mapTo, mergeMap, filter } from 'rxjs/operators';
import { GlobalizationStringService } from '../globalization/Services/globalization-string.service';

class TimeAgoRecord{
    private _time: Date = null;
    public lastUpdateTime: Date = null;
    public currentString: string = null;
    public strObs: Observable<string> = null;
    
    get time(): Date{
        return this._time;
    }

    constructor(time: Date){
       this._time = time;
    }
}

@Injectable({providedIn: 'root'})
export class TimeAgoPipeService {
    private _mainSubj: Subject<void> = new Subject<void>();
    private _mainThread: Observable<void> = this._mainSubj.pipe(tap(x => {
        const _curDate: Date = new Date();
        const sec: number = TimeAgoPipeService.getSeconds(_curDate);

        const updated: Record<number, TimeAgoRecord> = {};

        Object.keys(this._times).forEach(x => {
            if (this._times[x].lastUpdateTime == null || 
            TimeAgoPipeService.getUpdateInterval(_curDate, this._times[x].time) < 
            TimeAgoPipeService.getDiffSeconds(_curDate, this._times[x].lastUpdateTime)){
                updated[x] = this._times[x];
                this._times[x].currentString = TimeAgoPipeService.getStringName(
                    TimeAgoPipeService.getDiffSeconds(_curDate, this._times[x].time)
                );
                this._times[x].lastUpdateTime = new Date();
            }
        });
        console.log(updated);
        if (Object.keys(updated).length > 0)
            this._onNewString.next(updated);
    }));
    
    private _onNewString: BehaviorSubject<Record<number, TimeAgoRecord>> = new BehaviorSubject<Record<number, TimeAgoRecord>>({});

    private get _times(): Record<number, TimeAgoRecord>{
        return this._onNewString.value;
    }

    constructor(private _gbsService: GlobalizationStringService) {
        setInterval(() => {
            this._mainSubj.next();
        }, 3000);
        this._mainThread.subscribe();
    }

    public static getSeconds(time: Date): number{
        return Math.round(Math.abs(time.getTime()/1000))
    }

    public static getDiffSeconds(d1: Date, d2: Date): number{
        return Math.round(Math.abs((d1.getTime() - d2.getTime())/1000))
    }

    public static getStringName(seconds: number){
        let minutes = Math.round(Math.abs(seconds / 60));
		let hours = Math.round(Math.abs(minutes / 60));
		let days = Math.round(Math.abs(hours / 24));
		let months = Math.round(Math.abs(days/30.416));
		let years = Math.round(Math.abs(days/365));
		if (Number.isNaN(seconds)){
			return '';
		} else if (seconds <= 45) {
			return '[tmg-few-sec]';//'a few seconds ago';
		} else if (seconds <= 90) {
			return '[tmg-min]';
		} else if (minutes <= 45) {
			return `[tmg-mins, ${minutes}]`;
		} else if (minutes <= 90) {
			return '[tmg-hour]';//'an hour ago';
		} else if (hours <= 22) {
			return `[tmg-hours, ${hours}]`;//hours + ' hours ago';
		} else if (hours <= 36) {
			return '[tmg-day]'; //'day ago';
		} else if (days <= 25) {
			return `[tmg-days, ${days}]`; //days + ' days ago';
		} else if (days <= 45) {
			return '[tmg-month]'; //'month ago';
		} else if (days <= 345) {
			return `[tmg-months, ${months}]`; //months + ' months ago';
		} else if (days <= 545) {
			return '[tmg-year]'; //'year ago';
		} else { // (days > 545)
			return `[tmg-years, ${years}]`; //years + ' years ago';
		}
    }

    public static getUpdateInterval(curDate: Date, time: Date): number{
        const seconds: number = TimeAgoPipeService.getDiffSeconds(curDate, time);
        if (seconds < 60) 
            return 2; // less than 1 min, update every 2 secs
        else if (seconds < 60 * 60) 
            return 30; // less than an hour, update every 30 secs
        else if (seconds < 60 * 60 * 24) 
            return 300; // less then a day, update every 5 mins
        else return 3600;
    }

    public getStringObservable(time: Date): Observable<string>{
        
        const seconds: number = TimeAgoPipeService.getSeconds(time);
        if (isNaN(seconds)) return null;

        if (!this._times[seconds])
            this._times[seconds] = new TimeAgoRecord(time);
        if (!this._times[seconds].strObs)
            this._times[seconds].strObs = this._onNewString.pipe(
                filter(x => {
                    return Object.keys(x).includes(seconds.toString())
                }),
                map(x => x[seconds].currentString)
            );
        this._times[seconds].currentString = TimeAgoPipeService.getStringName(
            TimeAgoPipeService.getDiffSeconds(new Date(), this._times[seconds].time)
        );
        this._times[seconds].lastUpdateTime = new Date();
        
        return this._times[seconds].strObs;
    }
}