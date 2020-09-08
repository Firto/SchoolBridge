import { Injectable } from '@angular/core';  
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { GlobalizationInfo } from '../Models/globalization-info.model';
import { MyLocalStorageService } from 'src/app/Services/my-local-storage.service';
  
@Injectable()
export class GlobalizationInfoService {
    private _info: BehaviorSubject<GlobalizationInfo> = new BehaviorSubject<GlobalizationInfo>(null);
    private _infoObs: Observable<GlobalizationInfo> = this._info.asObservable();

    public get infoObs(): Observable<GlobalizationInfo> {
        return this._infoObs;
    }

    public get info(): GlobalizationInfo{
        return this._info.value;
    }
    public set info(val: GlobalizationInfo){
        this._localStorage.write("gbinfo", val);
        this._info.next(val);
    }

    constructor(private _localStorage: MyLocalStorageService){
        if (this._localStorage.isIssetKey('gbinfo')){
            this._info.next(this._localStorage.read("gbinfo"));
        }
    }
}    