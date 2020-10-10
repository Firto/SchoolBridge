import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { MyLocalStorageService } from 'src/app/Services/my-local-storage.service';
import { UserService } from 'src/app/Services/user.service';
import { DbStringDirective } from '../Directives/dbstring.directive';
import { HttpGlobalizationService } from './http-globalization.service';
import { GlobalizationStringService } from './globalization-string.service';
import { GlobalizationInfo } from '../Models/globalization-info.model';
import { GlobalizationInfoService } from './globalization-info.service';
import { tap } from 'rxjs/operators';
import { GlobalizationService } from './globalization.service';
import { GuardService } from 'src/app/Services/guard.service';

@Injectable()
export class GlobalizationEditService {
    private _state: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

    public get state(): boolean{
        return this._state.value;
    }

    public set state(val: boolean){
        this._state.next(val);
    }

    public get stateObs(): Observable<boolean>{
        return this._state;
    }
    private _editing: BehaviorSubject<any> = new BehaviorSubject<any>(null);

    public get editingObs(): Observable<any>{
        return this._editing;
    }

    constructor(private _localStorage: MyLocalStorageService,
                private _userService: UserService,
                private _gbsService: GlobalizationStringService,
                private _httpgbService: HttpGlobalizationService,
                private _gbiService: GlobalizationInfoService,
                private _guardService: GuardService){
        if (this._localStorage.isIssetKey('gbediting')){
            this.state = this._localStorage.read("gbediting");
        }

        this._state.subscribe(x => {
            this._guardService.setState(!x);
            this._localStorage.write("gbediting", x);
        })

        this._userService.userObs.subscribe(x => {
            if (!x)
                this.state = false;
        });
    }

    public changeEditing(directive: any){
        this._editing.next(directive);
    }

    public setString(name: string, str: string): Observable<any>
    {
        return this._httpgbService.setString(name, str).pipe(
                tap((x) => this._gbsService.setString(name, str))
            );
    }

    public updateBaseUpdateId(): Observable<GlobalizationInfo>{
        return this._httpgbService.updateBaseUpdateId().pipe(tap(x => {
            if (x.baseUpdateId != this._gbiService.info.baseUpdateId)
                this._gbsService.clearAllData();
            this._gbiService.info = x;
            this._gbsService.loadAllNoLoadedString();
        }));
    }

    public addLanguage(abbName: string, fullName: string): Observable<any>{
        return this._httpgbService.addLanguage(abbName, fullName).pipe(tap(res=> {
            this._gbiService.info.awaibleLanguages[abbName] = fullName;
            this._gbiService.info = this._gbiService.info;
        }));
    }

    public removeLanguage(abbName: string): Observable<any>{
        return this._httpgbService.removeLanguage(abbName).pipe(tap(res=> {
            delete this._gbiService.info.awaibleLanguages[abbName];
            this._gbiService.info = this._gbiService.info;
       }));
    }
}
