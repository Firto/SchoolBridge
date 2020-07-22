import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';


import { APIResult } from 'src/app/Models/api.result.model';
import { tap, map } from 'rxjs/operators';
import { UserService } from '../Services/user.service';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    constructor(private userService: UserService,
                private toastrService: ToastrService) { }

    intercept(request: HttpRequest<APIResult>, next: HttpHandler): Observable<HttpEvent<APIResult>> {
        return next.handle(request).pipe(
            map((event: HttpEvent<APIResult>) => {
                if (event instanceof HttpResponse){
                    if (event.body.ok)
                        return <any>event.clone({body: event.body.result});
                    switch (event.body.result.id){
                        case "inc-refresh-token":
                        case "already-login":
                        case "no-login":
                        case "base-account-service":
                        case "inc-uuid":
                        case "no-uuid":
                            if (this.userService.getUser() != null)
                                this.userService.localLogout();
                            break;
                        case "v-dto-invalid":
                        case "inc-token":
                            throw event.body.result;
                    }
                    this.toastrService.error(event.body.result.message, null, {timeOut: 10000});
                    throw event.body.result;
                }
                return event;
            }
        ));
    }
}