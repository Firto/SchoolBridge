import { Injectable } from '@angular/core';  
import { MyLocalStorageService } from 'src/app/Services/my-local-storage.service';
import { HttpGlobalizationService } from './http-globalization.service';
import { GlobalizationInfoService } from './globalization-info.service';
import { GlobalizationStringService } from './globalization-string.service';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { GlobalizationInfo } from '../Models/globalization-info.model';
  
@Injectable()
export class GlobalizationService {
    private _initedComponents: Record<string, {prefix: string, constStrings: string[]}> = {};

    constructor(private _localStorage: MyLocalStorageService,
                private _httpGbService: HttpGlobalizationService,
                private _gbiService: GlobalizationInfoService,
                public gbStringService: GlobalizationStringService){

        this._httpGbService.getInfo().subscribe(x => {
            if (this._gbiService.info && x.baseUpdateId != this._gbiService.info.baseUpdateId)
                this.gbStringService.clearAllData();
            this._gbiService.info = x;
        });
    }

    public localSetInfo(info: GlobalizationInfo){
        if (info.baseUpdateId != this._gbiService.info.baseUpdateId)
            this.gbStringService.clearAllData();
        this._gbiService.info = info;
    }

    public initComponent(name: string, prefix: string, constStrings: string[]){
        if (!Object.keys(this._initedComponents).includes(name)){
            this._initedComponents[name] = {prefix: prefix, constStrings: constStrings};
            this.gbStringService.initConstStrings(constStrings);
        }
    }

    public getComponent(name: string): {prefix: string, constStrings: string[]}{
        return Object.keys(this._initedComponents).includes(name) ? this._initedComponents[name] : null;
    }

    public changeLanguage(lang: string = ""): Observable<GlobalizationInfo>{
        return this._httpGbService.getInfo(lang).pipe(tap(x => {
            if (x.baseUpdateId != this._gbiService.info.baseUpdateId)
                this.gbStringService.clearAllData();
            this._gbiService.info = x;
            this.gbStringService.loadAllNoLoadedString();
        }));
    }
}    