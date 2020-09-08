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
                    Object.assign(this._stringD, r);
                    this._localStorage.write("gbdata", this._stringDATA);
                    x.forEach(m => {
                        if (!Object.keys(r).includes(m))
                            r[m] = this.getLoadedStringSave(m, "-none-");
                    })
                    Object.keys(r).forEach(m => {
                        if (Object.keys(this._initializedStrings).includes(m) && this._initializedStrings[m]) 
                            this._initializedStrings[m].next(r[m]);
                    });
                    return "";
                }));
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
        str.forEach(x => obss[x] = this.getStringObs(x))
        return obss;
    }

    public getStringObs(str: string): Observable<string>{
        if (!this.isInitializedString(str) || this._initializedStrings[str] == null)
            this._initializedStrings[str] = new BehaviorSubject<string>(this.getLoadedStringHttp(str, "-loading-"));
        return this._initializedStrings[str];
    }

    public convertString(str: string): Observable<string>{
        const regex = /\[([a-z-0-9]+)(\s?\,\s?.+)?\]/;
        const match = str.match(regex);
        if (!match || match.length === 0) 
            return Observable.throw("Error string "+ str);

        return this.getStringObs(match[1]).pipe(map(x => {
            if (match[2]){
                match[2].split(/\s?\,\s?/).forEach(r => {
                    if (!r || r.length === 0) return;

                    const match = r.match(regex);
                    if (!match || match.length === 0)
                        x = x.replace("$arg$", r);
                    else {
                        const m = this.getLoadedStringHttp(match[1], match[1]);
                        x = x.replace("$arg$", m.charAt(0).toLowerCase() + m.slice(1));
                    }
                })
            }
            return x.charAt(0).toUpperCase() + x.slice(1);
        }));
    }

    public setString(name: string, str: string){
        this._stringD[name] = str;
        this._localStorage.write("gbdata", this._stringDATA);
        if (Object.keys(this._initializedStrings).includes(name) && this._initializedStrings[name]) 
            this._initializedStrings[name].next(str);
    }

    public clearAllData(){
        this._stringDATA = {};
        this._localStorage.write("gbdata", this._stringDATA);
    }
}