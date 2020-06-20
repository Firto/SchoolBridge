import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { apiConfig } from 'src/app/Const/api.config';
import { APIResult } from 'src/app/Models/api.result.model';
import { BaseService } from './base.service';
import { Loginned } from 'src/app/Models/loginned.model';
import { Service } from 'src/app/Interfaces/Service/service.interface';
import { UserService } from './user.service';
import { SyncRequestHeader } from 'ts-sync-request';
import { LoginnedTokens } from '../Models/loginned-tokens';

@Injectable({ providedIn: 'root' })
export class LoginService {
    private ser: Service;
    private timerRefreshToken: number = null;

    constructor(private baseService: BaseService,
                private userService: UserService) {
           
        this.ser = apiConfig["login"];

        this.userService.user.subscribe((user) => {
            this.removeRefreshTokenTimer();
            if (user != null)
                this.timerRefreshToken = window.setTimeout(() => this.refreshTokenA().subscribe(), (user.login.tokens.expires - Math.round(new Date().getTime()/1000) - 60)* 1000)

        });
    }

    private removeRefreshTokenTimer() {
		if (this.timerRefreshToken) {
			window.clearTimeout(this.timerRefreshToken);
			this.timerRefreshToken = null;
		}
	}

    // security uuid
    
    login(login: string, password: string): Observable<APIResult> {
        return this.baseService.send(this.ser, "login", {login: login, password: password}, {headers: {'UUID':this.userService.uuid}}).pipe(map(res => {
            if (res.ok == true)
                this.userService.localLogin(<Loginned>res.result);
            return res;
        }));
    }

    refreshTokenA(): Observable<APIResult> {
        return this.baseService.send(this.ser, "refreshtoken", {refreshToken: this.userService.user.value.login.tokens.refreshToken}, {headers: {'UUID':this.userService.uuid}}).pipe(map(res => {
            if (res.ok == true)
                this.userService.localSetLoginTokens(<LoginnedTokens>res.result);
            else this.userService.localLogout();
            return res;
        }));
    }

    refreshToken(): APIResult {
        const res = this.baseService.sendSync(this.ser, "refreshtoken", {refreshToken: this.userService.user.value.login.tokens.refreshToken}, [new SyncRequestHeader("UUID", this.userService.uuid)]);
        if (res.ok == true){
            this.userService.localSetLoginTokens(<LoginnedTokens>res.result);
        }
        else this.userService.localLogout();
        return res;
    }

    logout(): Observable<APIResult> {
        return this.baseService.send(this.ser, "logout").pipe(map(res => {
            this.userService.localLogout();
            return res;
        }));
    }

    register(login: string, password: string, confirmPassword: string): Observable<APIResult> {
        return this.baseService.send(this.ser, "register", {login: login, password: password, confirmPassword: confirmPassword}, {headers: {'UUID':this.userService.uuid}}).pipe(map(res => {
            if (res.ok == true)
                this.userService.localLogin(<Loginned>res.result);
            return res;
        }));
    }
}