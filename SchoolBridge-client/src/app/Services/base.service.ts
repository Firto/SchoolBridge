import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Service } from 'src/app/Interfaces/Service/service.interface';
import { HttpClient, HttpHeaders, HttpParams, HttpEvent } from '@angular/common/http';
import { APIResult } from 'src/app/Models/api.result.model';
import { Observable, Subject } from 'rxjs';
import { SyncRequestService, SyncRequestHeader } from 'ts-sync-request';
import { ConnectionService } from 'ng-connection-service';

export interface HttpOptions{
    headers?: HttpHeaders | {
        [header: string]: string | string[];
    };
    observe?: 'body';
    params?: HttpParams | {
        [param: string]: string | string[];
    };
    reportProgress?: boolean;
    responseType?: 'json';
    withCredentials?: boolean;
}

@Injectable({ providedIn: 'root' })
export class BaseService {

    private most: Array<{obs: Subject<APIResult>, 
                        ser: Service, 
                        method:string,
                        body?:any, 
                        options?: HttpOptions}> = new Array<{obs: Subject<APIResult>, ser: Service, method:string, body?:any, options?: HttpOptions}>();

    private cnn: boolean = true;

    constructor(private http: HttpClient,
                private syncHttp: SyncRequestService,
                private connectionService: ConnectionService) {
        this.connectionService.monitor().subscribe((x) => {
            this.cnn = x;
            if (x) this.most.forEach((s) => this.send(s.ser, s.method, s.body, s.options).subscribe((m) =>  s.obs.next(m)));
        });
    }

    send(ser: Service, method:string, body?:any, options?: HttpOptions): Observable<APIResult> {
        if (this.cnn){
            switch  (ser.methods[method].type){
                case "POST":
                    return this.post(ser, method, body, options);
                case "GET":
                    return this.get(ser, method, options);
            }
        }

        const obs: Subject<APIResult> = new Subject<APIResult>()
        this.most.push({obs: obs, ser: ser, method:method, body:body, options: options});
        return obs;
    }

    post(ser: Service, method:string, body?:any, options?: HttpOptions): Observable<APIResult> {
        return this.http.post<APIResult>(environment.apiUrl+ser.url+ser.methods[method].url,body, options);
    }

    get(ser: Service, method:string, options?: HttpOptions): Observable<APIResult> {
        return this.http.get<APIResult>(environment.apiUrl+ser.url+ser.methods[method].url, options);
    }

    sendSync(ser: Service, method:string, body?:any, headers?: SyncRequestHeader[]): APIResult {
        switch  (ser.methods[method].type){
            case "POST":
                return this.postSync(ser, method, body, headers);
            case "GET":
                return this.getSync(ser, method, headers);
        }
    }

    postSync(ser: Service, method:string, body?:any, headers?: SyncRequestHeader[]): APIResult {
        return this.syncHttp.post<any, APIResult>(environment.apiUrl+ser.url+ser.methods[method].url, body, headers);
    }

    getSync(ser: Service, method:string, headers?: SyncRequestHeader[]): APIResult {
        return this.syncHttp.get<APIResult>(environment.apiUrl+ser.url+ser.methods[method].url, headers);
    }

}