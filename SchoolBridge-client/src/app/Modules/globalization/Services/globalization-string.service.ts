import { Subject, BehaviorSubject, Observable } from 'rxjs';
import { MyLocalStorageService } from 'src/app/Services/my-local-storage.service';
import { tap, bufferWhen, mergeMap, debounceTime, map, filter } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { UserModel } from '../../panel/Models/user.model';
import { User } from 'src/app/Models/user.model';
import { ShortUserModel } from '../../panel/Models/short-user.model';
import { HttpGlobalizationService } from './http-globalization.service';
import { GlobalizationInfoService } from './globalization-info.service';

@Injectable()
export class GlobalizationStringService {
    private _stringDATA: Record<string, Record<string, string>> = {};
    private get _stringD(){
        return this._gbInfoService.info ? this._stringDATA[this._gbInfoService.info.currentLanguage] : {};
    } 
    private _toggleMainGetBuffer: Subject<any> = <Subject<any>>(new Subject<any>()).pipe(debounceTime(50));
    private _mainGetThread: Subject<string> = null;

    private _initializedStrings: Record<string, BehaviorSubject<string>> = {};

    constructor(private _localStorage: MyLocalStorageService,
                private _httpGbService: HttpGlobalizationService,
                private _gbInfoService: GlobalizationInfoService) { 
        if (this._localStorage.isIssetKey("gbdata"))
            this._stringDATA = this._localStorage.read("gbdata");

        this._gbInfoService.infoObs.subscribe(x => {
            if (x){
                if (!Object.keys(this._stringDATA).includes(x.currentLanguage))
                    this._stringDATA[x.currentLanguage] = {};
            }   
        });

        this._mainGetThread = <Subject<string>>(new Subject<string>()).pipe(
            tap(x => this._toggleMainGetBuffer.next(null)),
            bufferWhen(() => this._toggleMainGetBuffer),
            mergeMap(x => {
                return this._httpGbService.getStrings(x).pipe(map(r => {
                    x.forEach(m => {
                        if (!Object.keys(r).includes(m))
                            r[m] = this.getLoadedStringSave(m, "-none-");
                    })
                    return r;
                }));
            }),
            map((x) => {
                Object.assign(this._stringD, x);
                this._localStorage.write("gbdata", this._stringDATA);
                Object.keys(x).forEach(r => {
                    if (Object.keys(this._initializedStrings).includes(r) && this._initializedStrings[r]) 
                        this._initializedStrings[r].next(x[r]);
                });
                return "";
            })
        )

        this._mainGetThread.subscribe();
    }

    public loadAllNoLoadedString(){
        Object.keys(this._initializedStrings).forEach(r => {
            if (!Object.keys(this._stringD).includes(r))
                this._mainGetThread.next(r);
            else if (this._initializedStrings[r]) 
                this._initializedStrings[r].next(this._stringD[r]);
        });
    }

    public getLoadedStringSave(str: string, noFund: string): string{
        return Object.keys(this._stringD).includes(str)? this._stringD[str] : noFund;
    }

    private isInitializedString(str: string){
        return Object.keys(this._initializedStrings).includes(str);
    }

    private isLoadedString(str: string){
        return Object.keys(this._stringD).includes(str);
    }

    private getLoadedStringHttp(str: string, noFund: string): string{
        if (Object.keys(this._stringD).includes(str))
            return this._stringD[str];
        else {
            this._mainGetThread.next(str);
            return noFund;
        }
    }

    public initConstStrings(str: string[]){
        str.forEach(x => {
            if (!this.isInitializedString(x))
                this._initializedStrings[x] = null;
            this.getLoadedStringHttp(x, "-loading-");
        })
    }

    public getStringsObs(str: string[]): Record<string, Observable<string>>{
        const obss: Record<string, Observable<string>> = {};
        str.forEach(x => {
            if (!this.isInitializedString(x) || this._initializedStrings[x] == null)
                this._initializedStrings[x] = new BehaviorSubject<string>(this.getLoadedStringHttp(x, "-loading-")); 
            obss[x] = this._initializedStrings[x];
        })
        return obss;
    }

    public convertString(str: string){
        const regex = /\[([a-z-]+)(\s?\,\s?.+)?\]/;
        const match = str.match(regex);
        console.log(match);
        if (!match || match.length === 0) return str;
        str = this.getLoadedStringSave(match[1], match[1]);
        if (match[2]){
            const mt = match[2].match(/\[([^\]\[]+)+\]/);
            if (mt[1])
                str = str.replace("$arg$", this.getLoadedStringSave(mt[1], mt[1]).toLowerCase());
        }

        return str;
    }

    public setString(name: string, str: string){
        this._stringD[name] = str;
        if (Object.keys(this._initializedStrings).includes(name) && this._initializedStrings[name]) 
            this._initializedStrings[name].next(str);
    }

    public clearAllData(){
        this._stringDATA = {};
        this._localStorage.write("gbdata", this._stringDATA);
    }
}