import { Injectable } from '@angular/core';
import { Service } from 'src/app/Interfaces/Service/service.interface';
import { BehaviorSubject, Observable, Subject, interval, forkJoin, of, throwError } from 'rxjs';
import { GlobalizationInfo } from 'src/app/Modules/globalization/Models/globalization-info.model';
import { BaseService } from 'src/app/Services/base.service';
import { UserService } from 'src/app/Services/user.service';
import { CryptService } from 'src/app/Services/crypt.service';
import { apiConfig } from 'src/app/Const/api.config';
import { map, takeUntil, take, tap, bufferCount, bufferTime, bufferWhen, filter, mergeMap, mapTo, concatAll, catchError, debounce, debounceTime, concatMap } from 'rxjs/operators';
import { APIResult } from 'src/app/Models/api.result.model';
import { DbStringDirective } from '../Directives/dbstring.directive';
import { UserError } from 'src/app/Models/user-error.model';
import { GuardService } from 'src/app/Services/guard.service';

@Injectable({ providedIn: 'root' })
export class GlobalizationService {
    private ser: Service;
    private _info: BehaviorSubject<GlobalizationInfo> = new BehaviorSubject<GlobalizationInfo>(null);
    private _isEditing: BehaviorSubject<Boolean> = new BehaviorSubject<Boolean>(false);
    private _stringEditWindow: Subject<DbStringDirective> = new Subject<DbStringDirective>();

    private _data: Record<string, Record<string, string>> = {};

    private _usedStrings: Array<string> = [];

    private _usedComponets: Record<string, {prefix: string, constStrings: string[]}> = {};

    // load logic
    private _toggleMainGetBuffer: Subject<any> = <Subject<any>>(new Subject<any>()).pipe(debounceTime(100));
    private _mainGetStringsThread: Subject<string> = null;

    private _stringsLoaded: Subject<Record<string, string>> = new Subject<Record<string, string>>();

    public changeEditing: Observable<Boolean> = this._isEditing.asObservable();
    public onStringEditWindow: Observable<DbStringDirective> = this._stringEditWindow.asObservable();

    public info: Observable<GlobalizationInfo> = this._info.asObservable();

    constructor(private baseService: BaseService,
                private userService: UserService,
                private _guardService: GuardService,
                private crypt: CryptService) {

        this.ser = apiConfig["globalization"];

        this._isEditing.subscribe(x => {
            this._guardService.setState(!x);
        });

        this.userService.user.subscribe(x => {
            if (!x)
                this.setEditingState(false);
        });

        if (localStorage.getItem('globalization_info')){
            //alert(this.crypt.decode(localStorage.getItem('globalization_info'), this.userService.uuid));
            
            try {
                this._info.next(JSON.parse(this.crypt.decode(localStorage.getItem('globalization_info'), this.userService.uuid)));
                this._info.value.awaibleLanguages = null;
                this._data = JSON.parse(this.crypt.decode(localStorage.getItem('globalization_data'), this.userService.uuid));
                this._stringsLoaded.next(this._data[this._info.value.currentLanguage]);
            }catch{
                this._data = {};
                if (this._info.value !== null)
                    this._data[this._info.value.currentLanguage] = {};
            }

            try {
                this.setEditingState(localStorage.getItem('globalization_editing') === '0' ? false : true);
            }catch{}
        }

        this._mainGetStringsThread = <Subject<string>>(new Subject<string>()).pipe(
            tap(x => this._toggleMainGetBuffer.next(null)),
            bufferWhen(() => this._toggleMainGetBuffer),
            mergeMap(x => {
                if (this._info.value && this._info.value.awaibleLanguages)
                    return this.saveGetStrings(x);
                else return this.getInfo(this._info.value ? this._info.value.currentLanguage : "").pipe(mergeMap(() => this.saveGetStrings(x)));
            }),
            tap((x) => this._stringsLoaded.next(x)),
            tap((x) => console.log('Loaded success strings', x)),
            mapTo('Loaded success strings')
        )

        this._mainGetStringsThread.subscribe();
    }

    public getLocalInfo(): GlobalizationInfo{
        return this._info.value;
    }

    public isIssetDataForCurrentLanguage(): Boolean {
        return this._info.value !== null && Object.keys(this._data).includes(this._info.value.currentLanguage);
    }

    public saveGetStrings(strings: string[]): Observable<Record<string, string>>{
        if (this.getNoLoadedStringsCur(strings).length > 0)
            return this.getStrings(this.getNoLoadedStringsCur(strings)).pipe(
                map((x) => {
                    return Object.assign(x, this.getLocalStrings(this.getLoadedStringsCur(strings)));
                })
            );
        else return of(this.getLocalStrings(strings));
    }

    public getStrings(strings: string[]): Observable<Record<string, string>>{
        return this.baseService.send<Record<string, string>>(this.ser, "strings", {strings: strings}, 
            {headers: {baseupdateid: this._info.value.baseUpdateId, "accept-language": this._info.value.currentLanguage}}).pipe(
            catchError(err => {
                if ('id' in err && err.id == 'lss-baseupateid-inc'){
                    return of(this.getInfo(this._info.value.currentLanguage), this.getStrings(strings)).pipe(
                        concatAll(),
                        bufferCount(2),
                        map(x => <Record<string, string>>x[1])
                    )
                }
                return throwError(err);
            }),
            tap(res => {
                this.localSetStrings(res);
            })
        );
    }

    public getEditingState(): Boolean{
        return this._isEditing.value;
    }

    public initComponent(name: string, prefix: string, strs: string[]){
        if (Object.keys(this._usedComponets).includes(name))
            return; 

        strs.forEach(x => {
            if (!this._usedStrings.includes(x)){
                this._usedStrings.push(x);
                this._mainGetStringsThread.next(x);
            }
        });

        this._usedComponets[name] = {prefix: prefix, constStrings: strs};
    }

    public setEditingState(st: Boolean){
        localStorage.setItem('globalization_editing', st ? '1' : '0');
        this._isEditing.next(st);
    }

    public localSetStrings(strs: Record<string, string>){
        Object.assign(this._data[this._info.value.currentLanguage], strs);
        this.saveSetData(this._data);
    }

    public getLocalStrings(names: string[]): Record<string, string> {
        const m: Record<string, string> = {};
        for (const item of names)
          m[item] = Object.keys(this._data[this._info.value.currentLanguage]).includes(item) === false ? '' : this._data[this._info.value.currentLanguage][item];
        return m;
    }

    public getNoLoadedStrings(): string[] {
        const m: string[] = [];
        for (const item of this.getUsedStrings()){
            if (Object.keys(this._data[this._info.value.currentLanguage]).includes(item) === false)
                m.push(item);
        }
        return m;
    }

    public getNoLoadedStringsCur(names: string[]): string[] {
        const m: string[] = [];
        for (const item of names){
            if (Object.keys(this._data[this._info.value.currentLanguage]).includes(item) === false)
                m.push(item);
        }
        return m;
    }

    public getLoadedStringsCur(names: string[]): string[] {
        const m: string[] = [];
        for (const item of names){
            if (Object.keys(this._data[this._info.value.currentLanguage]).includes(item) === true)
                m.push(item);
        }
        return m;
    }

    private saveSetInfo(i: GlobalizationInfo): void{
        localStorage.setItem('globalization_info', this.crypt.encode(JSON.stringify(i), this.userService.uuid));
    }

    private saveSetData(data: Record<string, Record<string, string>>): void{
        localStorage.setItem('globalization_data', this.crypt.encode(JSON.stringify(data), this.userService.uuid));
    }

    private getUsedStrings(): string[]{
        return this._usedStrings;
    }

    public initLocalStrings(...strings: string[]): Observable<Record<string, string>>{
        strings.forEach(x => {
            if (!this._usedStrings.includes(x)){
                this._usedStrings.push(x);
                this._mainGetStringsThread.next(x);
            }
        });
        return this._stringsLoaded.pipe(
            filter(x => Object.keys(x).some(v => strings.indexOf(v) !== -1)),
            map(x => {
                const rep: Record<string, string> = {};
                strings.forEach(r => rep[r] = x[r]);
                return rep;
            })
        );
    }

    public addLanguage(abbName: string, fullName: string): Observable<any>{
        return this.baseService.send<any>(this.ser, "add-language", {abbName, fullName}).pipe(tap(res=> {
            this._info.value.awaibleLanguages[res.result.abbName] = res.result.fullName;
            this._info.next(this._info.value);
            this.saveSetInfo(this._info.value);
        }));
    }

    public removeLanguage(abbName: string): Observable<any>{
        return this.baseService.get(this.ser, "remove-language", {params: {abbName: abbName}}).pipe(tap(res=> {
            delete this._info.value.awaibleLanguages[abbName];
            this._info.next(this._info.value);
            this.saveSetInfo(this._info.value);
       }));
    }

    public getFirstGlobalizationData(lang: string = ""): Observable<any>{
        return this.getInfo(lang).pipe(mergeMap(x => {
            return this.getStrings(["global"]).pipe(tap(x => {
                Object.assign(this._data[this._info.value.currentLanguage], x);
                this.saveSetData(this._data);
            }));
        }));
    }

    public changeLanguage(lang: string = ""): Observable<any>{
        return this.getInfo(lang).pipe(
            mergeMap(x => 
                this.saveGetStrings(this.getUsedStrings())
            ),
            tap(x => this._stringsLoaded.next(x))
        );
    }

    private getInfo(lang: string = ""): Observable<GlobalizationInfo> 
    {
        return this.baseService.send<GlobalizationInfo>(this.ser, "info", null, {headers: {"accept-language": lang}}).pipe(tap(x => {
            if (this._info.value !== null && x.baseUpdateId !== this._info.value.baseUpdateId)
                this._data = {};

            this._info.next(x); 
            if (Object.keys(this._data).includes(this._info.value.currentLanguage) === false)
                this._data[this._info.value.currentLanguage] = {};
            this.saveSetInfo(x);
        }));
    }

    public setString(name: string, str: string): Observable<GlobalizationInfo> 
    {
        return this.baseService.send<any>(this.ser, "add-or-upd-string", 
                                        {name: name, string: str}, 
                                        {headers: {"accept-language": this._info.value.currentLanguage}}
            ).pipe(
                tap(() => {
                    let r = {};
                    r[name] = str;
                    this._stringsLoaded.next(r);
                    this.localSetStrings(r);
                })
            );
    }
    
    public showStringEditWindow(directive: DbStringDirective){
        this._stringEditWindow.next(directive);
    }

}