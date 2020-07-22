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

@Injectable({ providedIn: 'root' })
export class LoginService {
    private ser: Service;
    //private timerRefreshToken: number = null;

    constructor(private baseService: BaseService,
                private userService: UserService,
                private router: Router) {
        
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
        return this.baseService.send<Loginned>(this.ser, "login", {login: login, password: password}, {headers: {'UUID':this.userService.uuid}}).pipe(
            tap(res => {
                this.userService.localLogin(res);
            },
            err => {
                if (err.id !== 'v-dto-invalid')
                    this.userService.localLogout();
            })
        );
    }

    refreshToken(): Observable<LoginnedTokens> {
        return this.baseService.send<LoginnedTokens>(this.ser, "refreshtoken", {refreshToken: this.userService.getUser().login.tokens.refreshToken}, {headers: {'UUID':this.userService.uuid, 'skip':'sk'}} ).pipe(
            tap(res => {
                this.userService.localSetLoginTokens(<LoginnedTokens>res);
            },
            err => {
                this.userService.localLogout();
            })
        );
    }

    createRefreshTokenRequest(): HttpRequest<APIResult> {
        return this.baseService.createRequest(
            this.ser, 
            "refreshtoken", 
            {refreshToken: this.userService.getUser().login.tokens.refreshToken},
            {
                headers: new HttpHeaders({'UUID':this.userService.uuid}),
                reportProgress: false,
                params: new HttpParams(),
                responseType: 'json' ,
                withCredentials: false
            },
            
        );
    }

    logout(): Observable<any> {
        return this.baseService.send<any>(this.ser, "logout").pipe(
            finalize(() => {
                this.userService.localLogout();
                this.router.navigateByUrl('/start');
            })
        );
    }

    register(login: string, password: string, confirmPassword: string): Observable<Loginned> {
        return this.baseService.send<Loginned>(this.ser, "register", {login: login, password: password, confirmPassword: confirmPassword}, {headers: {'UUID':this.userService.uuid}}).pipe(
            tap(res => {
                this.userService.localLogin(res);
            },
            err => {
                this.userService.localLogout();
            })
        );
    }
}