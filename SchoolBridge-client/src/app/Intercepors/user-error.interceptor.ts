import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';


import { APIResult } from 'src/app/Models/api.result.model';
import { tap } from 'rxjs/operators';
import { UserService } from '../Services/user.service';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    constructor(private userService: UserService,
                private toastrService: ToastrService) { }

    intercept(request: HttpRequest<APIResult>, next: HttpHandler): Observable<HttpEvent<APIResult>> {
        return next.handle(request).pipe(tap((event: HttpEvent<APIResult>) => {
            if (event instanceof HttpResponse && 
                event.body.ok == false){

                switch (event.body.result.id){
                    case "inc-refresh-token":
                    case "already-login":
                    case "no-login":
                    case "base-account-service":
                    case "inc-uuid":
                    case "no-uuid":
                        if (this.userService.user.value != null)
                            this.userService.localLogout();
                        break;
                    case "v-dto-invalid":
                        return;
                }
                
                this.toastrService.error(event.body.result.message, null, {timeOut: 10000});
            }
        }));
    }
}