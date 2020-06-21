import { Injectable } from '@angular/core';
import { Service } from 'src/app/Interfaces/Service/service.interface';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { GlobalizationInfo } from 'src/app/Modules/globalization/Models/globalization-info.model';
import { BaseService } from 'src/app/Services/base.service';
import { UserService } from 'src/app/Services/user.service';
import { CryptService } from 'src/app/Services/crypt.service';
import { apiConfig } from 'src/app/Const/api.config';
import { map } from 'rxjs/operators';
import { APIResult } from 'src/app/Models/api.result.model';

@Injectable({ providedIn: 'root' })
export class GlobalizationService {
    private ser: Service;

    private _info: BehaviorSubject<GlobalizationInfo> = new BehaviorSubject<GlobalizationInfo>(null);
    public info: Observable<GlobalizationInfo> = this._info.asObservable();

    private data: Record<string, Record<string, string>> = {}; 

    constructor(private baseService: BaseService,
                private userService: UserService,
                private crypt: CryptService) {
        this.ser = apiConfig["globalization"];

        if (localStorage.getItem('globalization_info')){
            //alert(this.crypt.decode(localStorage.getItem('globalization_info'), this.userService.uuid));
            
            try {
                this._info.next(JSON.parse(this.crypt.decode(localStorage.getItem('globalization_info'), this.userService.uuid)));
            
                this.data = JSON.parse(this.crypt.decode(localStorage.getItem('globalization_data'), this.userService.uuid));
            }
            catch{
                
            }
        }           

        if (this._info.value == null) {
            this.getFirstGlobalizationData();            
        }
    }

    private saveSetInfo(i: GlobalizationInfo): void{
        localStorage.setItem('globalization_info', this.crypt.encode(JSON.stringify(i), this.userService.uuid));
    }

    private saveSetData(data: Record<string, Record<string, string>>): void{
        localStorage.setItem('globalization_data', this.crypt.encode(JSON.stringify(data), this.userService.uuid));
    }

    public addLanguage(abbName: string, fullName: string): Observable<APIResult>{
       return this.baseService.post(this.ser, "add-language", {abbName, fullName}).pipe(map(res=> {
            if (res.ok){
                this._info.value.awaibleLanguages[res.result.abbName] = res.result.fullName;
                this._info.next(this._info.value);
                this.saveSetInfo(this._info.value);
            }
            return res;
       }));
    }

    public removeLanguage(abbName: string): Observable<APIResult>{
        return this.baseService.get(this.ser, "remove-language", {params: {abbName: abbName}}).pipe(map(res=> {
            delete this._info.value.awaibleLanguages[abbName];
            this._info.next(this._info.value);
            this.saveSetInfo(this._info.value);
            return res;
       }));
    }

    public getFirstGlobalizationData(lang: string = ""){
        this.getInfo(lang).subscribe(x => {
            console.log(x);
            this._info.next(x); 
            this.saveSetInfo(x);
            this.getStrings(["global"]).subscribe(x => {
                this.data[this._info.value.currentLanguage] = {};
                Object.assign(this.data[this._info.value.currentLanguage], x);
                this.saveSetData(this.data);
            });
        });
    }

    private getInfo(lang: string = ""): Observable<GlobalizationInfo> 
    {
        const sb = new Subject<GlobalizationInfo>();
        this.baseService.get(this.ser, "info", {headers: {"accept-language": lang}}).subscribe(x => {
            if (x.ok)
                sb.next(x.result)
        });
        return sb;
    }

    private getStrings(types: string[]): Observable<Record<string, string>> 
    {
        const sb = new Subject<Record<string, string>>();
        this.baseService.post(this.ser, "strings", {types}, {headers: {baseupdateid: this._info.value.baseUpdateId, "accept-language": this._info.value.currentLanguage}}).subscribe(x => {
            if (x.ok)
                sb.next(x.result)
        });
        return sb;
    }
    

}