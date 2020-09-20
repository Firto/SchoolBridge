import { Injectable, Injector } from '@angular/core';
import { Observable, interval, Subject, BehaviorSubject, of } from 'rxjs';
import { map, takeUntil, tap, mapTo, mergeMap, filter } from 'rxjs/operators';
import { GlobalizationInfoService } from '../globalization/Services/globalization-info.service';
import { GlobalizationStringService } from '../globalization/Services/globalization-string.service';

class TimeAgoRecord{
    private _time: Date = null;
    private _strObs: Observable<string> = null;

    public lastUpdateTime: Date = null;
    
    get time(): Date{
        return this._time;
    }

    get strObs(): Observable<string>{
        return this._strObs;
    }

    constructor(time: Date, strObs: Observable<string>){
       this._time = time;
       this._strObs = strObs;
    }
}

@Injectable({providedIn: 'root'})
export class TimeAgoDirectiveService {

    private _mainThread: Observable<number> = interval(3000).pipe(tap(x => {
        const _curDate: Date = new Date();
        const sec: number = TimeAgoDirectiveService.getSeconds(_curDate);

        const updated: Record<number, string> = {};

        Object.keys(this._times).forEach(x => {
            if (this._times[x].lastUpdateTime == null || 
            TimeAgoDirectiveService.getUpdateInterval(_curDate, this._times[x].time) < 
            TimeAgoDirectiveService.getDiffSeconds(_curDate, this._times[x].lastUpdateTime)){
                updated[x] = TimeAgoDirectiveService.getStringName(
                    TimeAgoDirectiveService.getDiffSeconds(_curDate, this._times[x].time)
                );
                this._times[x].lastUpdateTime = _curDate;
            }
        });

        if (Object.keys(updated).length > 0){
            this._onNewString.next(updated);
        }
    }));
    
    private _onNewString: BehaviorSubject<Record<number, string>> = new BehaviorSubject<Record<number, string>>({});

    private readonly _constStrings: string[] = [
        "tmg-few-sec",
        "tmg-min",
        "tmg-mins",
        "tmg-hour",
        "tmg-hours",
        "tmg-day",
        "tmg-days",
        "tmg-month",
        "tmg-months",
        "tmg-year",
        "tmg-years"
    ];

    private _times: Record<number, TimeAgoRecord> = {};

    private get _newString(): Record<number, string>{
        return this._onNewString.value;
    }

    constructor(private _gbiService: GlobalizationInfoService) {
        this._mainThread.subscribe();

        this._gbiService.infoObs.subscribe(x => {
            if (x){
                const dt = new Date();
                const mm = {};
                Object.keys(this._times).forEach(r => mm[r] = TimeAgoDirectiveService.getStringName(
                    TimeAgoDirectiveService.getDiffSeconds(dt, this._times[r].time)
                ))
                this._onNewString.next(mm);
            }
        });
    }

    public static getSeconds(time: Date): number{
        return Math.round(Math.abs(time.getTime()/1000))
    }

    public static getDiffSeconds(d1: Date, d2: Date): number{
        return Math.round(Math.abs((d1.getTime() - d2.getTime())/1000))
    }

    public getConstStrings(): string[]{
        return this._constStrings;
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
        const seconds: number = TimeAgoDirectiveService.getDiffSeconds(curDate, time);
        if (seconds < 60) 
            return 2; // less than 1 min, update every 2 secs
        else if (seconds < 60 * 60) 
            return 5; // less than an hour, update every 30 secs
        else if (seconds < 60 * 60 * 24) 
            return 60; // less then a day, update every 5 mins
        else return 3600;
    }

    public getUsedStrings(){
        
    }

    public getStringObservable(time: Date): Observable<string>{
        const seconds: number = TimeAgoDirectiveService.getSeconds(time);

        if (!this._times[seconds]){
            this._times[seconds] = new TimeAgoRecord(
                                            time, 
                                            this._onNewString.pipe(
                                                filter(x => {
                                                    return Object.keys(x).includes(seconds.toString())
                                                }),
                                                map(x => x[seconds])
                                            )
                                    );
        }

        const curDate: Date = new Date(); 

        this._newString[seconds] = TimeAgoDirectiveService.getStringName(
            TimeAgoDirectiveService.getDiffSeconds(curDate, this._times[seconds].time)
        );

        return this._times[seconds].strObs;
    }
}