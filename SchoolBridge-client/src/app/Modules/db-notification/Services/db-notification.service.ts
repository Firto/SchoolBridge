import { Injectable, ComponentFactoryResolver, ComponentFactory } from '@angular/core';
import { Service } from 'src/app/Interfaces/Service/service.interface';
import { BaseService } from 'src/app/Services/base.service';
import { apiConfig } from 'src/app/Const/api.config';
import { Observable, Subject, BehaviorSubject } from 'rxjs';
import { APIResult } from 'src/app/Models/api.result.model';
import { NotificationService } from 'src/app/Modules/notification/Services/notification.service';
import { DataBaseSource } from '../../notification/Models/NotificationSources/database-source';
import { OnReadNtfSource } from '../Models/onreadntf-source';
import { UserService } from 'src/app/Services/user.service';
import { KeyedCollection } from 'src/app/Collections/keyed-collection';
import { Guid } from 'guid-typescript';

@Injectable({ providedIn: 'root' })
export class DbNotificationService {
    private ser: Service;
    
    private _onReciveDbNotification: Subject<{key: string, value: DataBaseSource, reverse: boolean}> = new Subject<{key: string, value: DataBaseSource, reverse: boolean}>();
    private _onReciveOnReadDbNotification: Subject<OnReadNtfSource> = new Subject<OnReadNtfSource>();
    private _countUnreadNtfs: BehaviorSubject<number> = new BehaviorSubject<number>(0);

    public onReciveDbNotification: Observable<{key: string, value: DataBaseSource, reverse: boolean}> = this._onReciveDbNotification.asObservable();
    public onReciveOnReadDbNotification: Observable<OnReadNtfSource> = this._onReciveOnReadDbNotification.asObservable();
    public onUpdateCountUnreadNtfs: Observable<number> = this._countUnreadNtfs.asObservable();

    public notificationList: KeyedCollection<string, DataBaseSource> = new KeyedCollection<string, DataBaseSource>();

    constructor(private baseService: BaseService,
                private userService: UserService,
                private ntfService: NotificationService) {
        this.ser = apiConfig["notification"];

        this.ntfService.reciveNotification.subscribe((data) => {
            switch (data.type){
                case "dataBase":
                    this._countUnreadNtfs.next(this._countUnreadNtfs.value + 1);
                    this.localAddNotifications(<DataBaseSource>data.source);
                  break;
                case "onReadNtf":
                    const som: OnReadNtfSource = <OnReadNtfSource>data.source;
                    for (let ind = this.notificationList.getIndex(som.last); ind >= 0; ind--) 
                        if (this.notificationList.items[ind].value.id != null) 
                            this.notificationList.items[ind].value.read = true;
                    this._countUnreadNtfs.next(this._countUnreadNtfs.value - som.count);
                    this._onReciveOnReadDbNotification.next(<OnReadNtfSource>som);
                break;
              }
        });

        this.userService.user.subscribe(x => {
            if (x){
                this.notificationList.clear();
                this.getCountUnread().subscribe(s => {
                    if (s.ok)
                        this._countUnreadNtfs.next(s.result);
                });
            }
        });

        /*if (this.userService.user.value){
            
            this.getCountUnread().subscribe(s => {
                if (s.ok)
                    this._countUnreadNtfs.next(s.result);
            });
        }
        
        this.userService.user.subscribe(x => {
            if (x){
                this.notificationList.clear();
                this._countUnreadNtfs.next(x.login.countUnreadNotifications);
            }
        });*/
    }

    public readedNtfs(){
        if (this.notificationList.items.some((x) => x.value.read == false && x.value.id != null))
            this.read(this.getLastNtfId()).subscribe();
        this.notificationList.items.forEach((x) => {
            if (x.value.read == false && x.value.id == null){
                x.value.read = true;
                this._countUnreadNtfs.next(this._countUnreadNtfs.value - 1);
            }
        });
    }

    public getNtfs(): Observable<boolean> {
        const sub: Subject<boolean> = new Subject<boolean>();
        this.getn(this.getLastNtfId()).subscribe((data) => {
            let end: boolean = false;
            if (data.ok) {
                const res: DataBaseSource[] = <DataBaseSource[]>data.result;
                if (res.length > 0) {
                    res.forEach(baseSource => this.localAddNotifications(baseSource, false));
                    if (res.length < 20)
                    end = true;
                }
                else end = true;
            }
            sub.next(end);
        });
        return sub;
    }

    public localAddNotifications(source: DataBaseSource, reverse: boolean = true){
        const id: string = source.id == null ? Guid.create().toString() : source.id;
        if (reverse)
            this.notificationList.addOrUpdateShift(id, source);
        else this.notificationList.addOrUpdate(id, source);
        this._onReciveDbNotification.next({key: id, value: source, reverse: reverse});
    }

    public localUpdateCountUnreadNtfs(){
        this._countUnreadNtfs.next(this._countUnreadNtfs.value);
    }

    public getLastNtfId(): string {
        const ntfs: Array<DataBaseSource> = this.notificationList.items.filter(x => x.value.id != null).map(x => x.value);
        return ntfs.length > 0 ? ntfs[ntfs.length-1].id : null;
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