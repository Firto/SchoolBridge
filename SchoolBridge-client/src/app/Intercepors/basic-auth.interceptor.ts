import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpClient, HttpResponse } from '@angular/common/http';
import { Observable, of, forkJoin, throwError, BehaviorSubject } from 'rxjs';

import { UserService } from 'src/app/Services/user.service';
import { LoginService } from '../Services/login.service';
import { APIResult } from '../Models/api.result.model';
import { delayWhen, map, tap, concatAll, bufferCount, concat, mergeMap, concatMap, switchMap, catchError, take, filter } from 'rxjs/operators';
import { BaseService } from '../Services/base.service';
import { LoginnedTokens } from '../Models/loginned-tokens';
import { UserError } from '../Models/user-error.model';

@Injectable()
export class BasicAuthInterceptor implements HttpInterceptor {
    constructor(private userService: UserService,
                private loginService: LoginService,
                private httpClient: HttpClient) { }

    private isRefreshing = false;
    private refreshTokenSubject: BehaviorSubject<string> = new BehaviorSubject<string>(null);

    intercept(request: HttpRequest<APIResult>, next: HttpHandler): Observable<HttpEvent<any>> {
        if (request.headers.has('skip')) 
            return next.handle(request);
        
        if (this.userService.getUser()){
            return next.handle(this.attachTokenToRequest(request)).pipe(
                catchError((err: any, obs: Observable<HttpEvent<any>>) => {
                    if ('id' in err && err.id === 'inc-token'){
                        if (!this.isRefreshing) {
                            this.isRefreshing = true;
                            this.refreshTokenSubject.next(null);
                            return this.loginService.refreshToken().pipe(switchMap((token: LoginnedTokens) => {
                                this.isRefreshing = false;
                                this.refreshTokenSubject.next(token.token);
                                return next.handle(this.attachTokenToRequest(request));
                            }));
                        }else return this.refreshTokenSubject.pipe(
                            filter(token => token != null),
                            take(1),
                            switchMap(jwt => {
                              return next.handle(this.attachTokenToRequest(request));
                            })
                        );
                    }
                    return throwError(err);
                }
            ));
        }

        return next.handle(request);
    }

    private attachTokenToRequest(request: HttpRequest<any>) {
        if (this.userService.getUser()) {
          return request.clone({
            setHeaders: { Authorization: `Bearer ${this.userService.getUser().login.tokens.token}` },
          });
        }
        return request;
      }
}