import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';

import { UserService } from 'src/app/Services/user.service';
import { LoginService } from '../Services/login.service';
import { APIResult } from '../Models/api.result.model';

@Injectable()
export class BasicAuthInterceptor implements HttpInterceptor {
    constructor(private userService: UserService,
                private loginService: LoginService) { }

    intercept(request: HttpRequest<APIResult>, next: HttpHandler): Observable<HttpEvent<APIResult>> {
        let currentUser = this.userService.user.value;
        if (currentUser != null) {
            
            if (currentUser.login.tokens.expires-10 < Math.round(new Date().getTime()/1000)){
                if (this.loginService.refreshToken().ok) 
                    currentUser = this.userService.user.value;
                else return Observable.create();
            }

            request = request.clone({
                setHeaders: { 
                    Authorization: `Bearer ${currentUser.login.tokens.token}`
                }
            });
        }
        return next.handle(request);
    }
}