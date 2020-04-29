import { Injectable, ComponentFactoryResolver, ComponentFactory } from '@angular/core';
import { Service } from 'src/app/Interfaces/Service/service.interface';
import { BaseService } from 'src/app/Services/base.service';
import { apiConfig } from 'src/app/Const/api.config';
import { Observable, Subject } from 'rxjs';
import { APIResult } from 'src/app/Models/api.result.model';
import { NotificationService } from 'src/app/Services/notification.service';
import { DataBaseSource } from '../Sources/database-source';
import { OnReadNtfSource } from '../Sources/onreadntf-source';

@Injectable()
export class DbNotificationService {
    private ser: Service;
    
    private _onReciveDbNotification: Subject<DataBaseSource> = new Subject<DataBaseSource>();
    private _onReciveOnReadDbNotification: Subject<OnReadNtfSource> = new Subject<OnReadNtfSource>();
    public onReciveDbNotification: Observable<DataBaseSource> = this._onReciveDbNotification.asObservable();
    public onReciveOnReadDbNotification: Observable<OnReadNtfSource> = this._onReciveOnReadDbNotification.asObservable();

    constructor(private baseService: BaseService,
                private ntfService: NotificationService) {
        this.ser = apiConfig["notification"];

        this.ntfService.reciveNotification.subscribe((data) => {
            switch (data.type){
                case "dataBase":
                    this._onReciveDbNotification.next(<DataBaseSource>data.source);
                  break;
                case "onReadNtf":
                    this._onReciveOnReadDbNotification.next(<OnReadNtfSource>data.source);
                break;
              }
        });
    }

    public read(last: string = null): Observable<APIResult> {
        return last == null ? this.baseService.send(this.ser, "read") : this.baseService.send(this.ser, "read", null, { params: { last: last } });
    }

    public getn(last: string = null): Observable<APIResult> {
        return last == null ? this.baseService.send(this.ser, "get") : this.baseService.send(this.ser, "get", null, { params: { last: last } });
    }

    public getAndRead(last: string = null): Observable<APIResult> {
        return last == null ? this.baseService.send(this.ser, "getandread") : this.baseService.send(this.ser, "getandread", null, { params: { last: last } });
    }

    public getCountUnread(): Observable<APIResult> {
        return this.baseService.send(this.ser, "getcountunread");
    }
}