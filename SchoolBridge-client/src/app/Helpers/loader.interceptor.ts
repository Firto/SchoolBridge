import { Injectable } from "@angular/core";
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Observable } from "rxjs";
import { finalize } from "rxjs/operators";
import { LoaderService } from './loader.service';
import { environment } from 'src/environments/environment';
import { apiConfig } from 'src/app/Const/api.config';
import { Service } from 'src/app/Interfaces/Service/service.interface';
import { Method } from 'src/app/Interfaces/Service/service.method.interface';

@Injectable()
export class LoaderInterceptor implements HttpInterceptor {
    constructor(public loaderService: LoaderService) { }
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        let show:Boolean = true;
        const match: RegExpMatchArray = req.url.match(new RegExp(`^(${environment.apiUrl})(.+\\/)(.+\\/?)$`));
        for (const [key, value] of Object.entries(apiConfig)) {
            const ser: Service = <Service>value;
            if (ser.url == match[2])
            {
                for (const [key, value] of Object.entries(ser.methods)) {
                    const met: Method = <Method>value;
                    if (met.url == match[3])
                    {
						if ('loader' in met)
							show = met.loader;
                        break;
                    }
                }
                break;
            }
        }
        if (show){
            this.loaderService.show();
            return next.handle(req).pipe(
                finalize(() => this.loaderService.hide())
            );
        }else return next.handle(req);
    }
}