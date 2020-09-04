import { Injectable, Injector } from '@angular/core';
import { Observable, BehaviorSubject, Subject } from 'rxjs';
import { map, takeUntil, take, tap } from 'rxjs/operators';

import { apiConfig } from 'src/app/Const/api.config';
import { APIResult } from 'src/app/Models/api.result.model';
import { BaseService } from '../../../Services/base.service';
import { Loginned } from 'src/app/Models/loginned.model';
import { Service } from 'src/app/Interfaces/Service/service.interface';

import { UserService } from 'src/app/Services/user.service';
import { ProfileModel } from '../Models/profile.model';


@Injectable()
export class ProfileService {
    private _ser: Service;
    private _info: BehaviorSubject<ProfileModel> = new BehaviorSubject<ProfileModel>(null);
    public info: Observable<ProfileModel> = this._info.asObservable();

    constructor(private baseService: BaseService,
                private userService: UserService) {
       
        this._ser = apiConfig["profile"];
        this.userService.userObs.subscribe((user) => {
            if (user != null)
                this.getInfo();
        });
        if (this.userService.user != null) {
            this.getInfo().subscribe();
        }
    }

    changeLogin(login: string): Observable<any> {
        return this.baseService.send<any>(this._ser, "change-login", { login }).pipe(tap(res => {
            this._info.value.login = login;
            this._info.next(this._info.value)
        }));
    }

    getInfo(): Observable<ProfileModel> {
        return this.baseService.send<ProfileModel>(this._ser, "info").pipe(tap(res => {
            this._info.next(<ProfileModel>res);
        }));
    }
}