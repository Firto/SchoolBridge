import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map, tap, catchError, finalize } from 'rxjs/operators';

import { apiConfig } from 'src/app/Const/api.config';
import { APIResult } from 'src/app/Models/api.result.model';
import { BaseService } from './base.service';
import { Loginned } from 'src/app/Models/loginned.model';
import { Service } from 'src/app/Interfaces/Service/service.interface';
import { UserService } from './user.service';
import { LoginnedTokens } from '../Models/loginned-tokens';
import { HttpHeaders, HttpRequest, HttpParams } from '@angular/common/http';
import { logging } from 'protractor';
import { Router } from '@angular/router';
import { DeviceUUIDService } from './device-uuid.service';

@Injectable()
export class LoginService {
    private ser: Service;
    //private timerRefreshToken: number = null;

    constructor(private _baseService: BaseService,
                private _userService: UserService,
                private _uuidService: DeviceUUIDService) {
        
        this.ser = apiConfig["login"];
        //this.userService.user.subscribe((user) => {
        //    this.removeRefreshTokenTimer();
        //    if (user != null)
        //        this.timerRefreshToken = window.setTimeout(() => this.refreshToken().subscribe(), (user.login.tokens.expires - Math.round(new Date().getTime()/1000) - 60)* 1000)

        //});
    }

    /*private removeRefreshTokenTimer() {
		if (this.timerRefreshToken) {
			window.clearTimeout(this.timerRefreshToken);
			this.timerRefreshToken = null;
		}
	}*/

    // security uuid
    
    login(login: string, password: string): Observable<Loginned> {
        return this._baseService.send<Loginned>(this.ser, "login", {login: login, password: password}, 
            {headers: {'UUID': this._uuidService.uuid}}).pipe(
            tap(res => {
                this._userService.localLogin(res);
            },
            err => {
                if (err.id !== 'v-dto-invalid')
                    this._userService.localLogout();
            })
        );
    }

    refreshToken(): Observable<LoginnedTokens> {
        return this._baseService.send<LoginnedTokens>(this.ser, "refreshtoken", {refreshToken: this._userService.user.login.tokens.refreshToken}, {headers: {'UUID':this._uuidService.uuid, 'skip':'sk'}} ).pipe(
            tap(res => {
                this._userService.localSetLoginTokens(<LoginnedTokens>res);
            },
            err => {
                this._userService.localLogout();
            })
        );
    }

    createRefreshTokenRequest(): HttpRequest<APIResult> {
        return this._baseService.createRequest(
            this.ser, 
            "refreshtoken", 
            {refreshToken: this._userService.user.login.tokens.refreshToken},
            {
                headers: new HttpHeaders({'UUID':this._uuidService.uuid}),
                reportProgress: false,
                params: new HttpParams(),
                responseType: 'json' ,
                withCredentials: false
            },
            
        );
    }

    logout(): Observable<any> {
        return this._baseService.send<any>(this.ser, "logout").pipe(
            finalize(() => {
                this._userService.localLogout();
                //this.router.navigateByUrl('/start');
            })
        );
    }

    register(login: string, password: string, confirmPassword: string): Observable<Loginned> {
        return this._baseService.send<Loginned>(this.ser, "register", {login: login, password: password, confirmPassword: confirmPassword}, {headers: {'UUID':this._uuidService.uuid}}).pipe(
            tap(res => {
                this._userService.localLogin(res);
            },
            err => {
                this._userService.localLogout();
            })
        );
    }
}