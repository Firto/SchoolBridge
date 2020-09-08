import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Service } from 'src/app/Interfaces/Service/service.interface';
import { HttpClient, HttpHeaders, HttpParams, HttpRequest } from '@angular/common/http';
import { APIResult } from 'src/app/Models/api.result.model';
import { Observable, Subject, BehaviorSubject } from 'rxjs';
import { ConnectionService } from 'ng-connection-service';
import { take } from 'rxjs/operators';

export interface HttpOptions{
    headers?: HttpHeaders;
    reportProgress?: boolean;
    params?: HttpParams;
    responseType?: 'json' | 'arraybuffer' | 'blob' | 'text';
    withCredentials?: boolean;
}

export interface HttpOptionsWide{
    reportProgress?: boolean;
    withCredentials?: boolean;
    observe?: 'body';
    responseType?: 'json';
    headers?: HttpHeaders | {
        [header: string]: string | string[];
    };
    params?: HttpParams | {
        [param: string]: string | string[];
    };
}

@Injectable()
export class BaseService {

    /*private most: Array<{obs: Subject<APIResult>, 
                        ser: Service, 
                        method:string,
                        body?:any, 
                        options?: HttpOptionsWide}> = new Array<{obs: Subject<APIResult>, ser: Service, method:string, body?:any, options?: HttpOptionsWide}>();
*/
    //private cnn: boolean = true;

    private _state: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(true);
    public state: Observable<boolean> = this._state.asObservable();

    constructor(private http: HttpClient) {
        /*this.connectionService.monitor().subscribe((x) => {
            this.cnn = x;
            if (x) this.most.forEach((s) => this.send(s.ser, s.method, s.body, s.options).subscribe((m) =>  s.obs.next(m)));
        });*/ 
    }

    setState(state: boolean) {
        this._state.next(state);
    }

    getState(): boolean {
        return this._state.value;
    }

    send<T>(ser: Service, method:string, body?:any, options?: HttpOptionsWide): Observable<T> {
        //if (!this._state.value) return;
        let m;
        switch  (ser.methods[method].type){
            case "POST":
                return <Observable<T>>this.post(ser, method, body, options).pipe(take(1));
            case "GET":
                return <Observable<T>>this.get(ser, method, options).pipe(take(1));
        }
        /*if (this.cnn){
           
        }

        const obs: Subject<APIResult> = new Subject<APIResult>()
        this.most.push({obs: obs, ser: ser, method:method, body:body, options: options});
        return <Observable<T>><unknown>obs;*/
    }

    createRequest(ser: Service, method:string, body?:any, options?: HttpOptions){
        return new HttpRequest<APIResult>(ser.methods[method].type, environment.apiUrl+ser.url+ser.methods[method].url, body, options);
    }

    post(ser: Service, method:string, body?:any, options?: HttpOptionsWide): Observable<APIResult> {
        return this.http.post<APIResult>(environment.apiUrl+ser.url+ser.methods[method].url, body, options);
    }

    get(ser: Service, method:string, options?: HttpOptionsWide): Observable<APIResult> {
        return this.http.get<APIResult>(environment.apiUrl+ser.url+ser.methods[method].url, options);
    }

}