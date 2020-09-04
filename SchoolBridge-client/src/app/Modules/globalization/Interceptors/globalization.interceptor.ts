import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';

import { APIResult } from 'src/app/Models/api.result.model';
import { catchError } from 'rxjs/operators';
import { GlobalizationStringService } from '../Services/globalization-string.service';

@Injectable()
export class GlobalizationInterceptor implements HttpInterceptor {
    constructor(private _gbsService: GlobalizationStringService) { }

    intercept(request: HttpRequest<APIResult>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(request).pipe(
            catchError((err: any, obs: Observable<HttpEvent<any>>) => {
                if ('id' in err){
                    err.message = this._gbsService.convertString(err.message);
                }
                return throwError(err);
            })
        );
    }
}