import { Injectable, ComponentFactoryResolver, ComponentFactory } from '@angular/core';
import { Service } from 'src/app/Interfaces/Service/service.interface';
import { BaseService } from 'src/app/Services/base.service';
import { apiConfig } from 'src/app/Const/api.config';
import { Observable, Subject, BehaviorSubject, of } from 'rxjs';
import { NotificationService } from 'src/app/Modules/notification/Services/notification.service';
import { DataBaseSource } from '../../notification/Models/NotificationSources/database-source';
import { UserService } from 'src/app/Services/user.service';
import { KeyedCollection } from 'src/app/Collections/keyed-collection';
import { Guid } from 'guid-typescript';
import { OnReadNtfSource } from '../../notification/Models/NotificationSources/on-read-ntf.source';
import { map, take, tap } from 'rxjs/operators';
import { fromBinary } from '../../binary/from-binary.func';
import { IDBNSource } from '../Models/IDBN-source.interface';

export class DbNotification<T>{
    public readonly source: T;
    public readonly date: Date;
    get id(): string{
        return this.model.id;
    }
    get read(): boolean{
        return this.model.read;
    }
    set read(val: boolean){
        this.model.read = val;
    }
    get type(): string{
        return this.model.type;
    } 
    constructor(public readonly model: DataBaseSource){
        this.source = JSON.parse(fromBinary(this.model.base64Sourse));
        this.date = new Date(this.model.date);
    }
}

@Injectable({providedIn: 'root'})
export class DbNotificationService { 
    private ser: Service;
    private _notifications: Record<string, DbNotification<IDBNSource>> = {};
    private __notifications: DbNotification<IDBNSource>[] = [];
    private readonly _dbNotificatinsRecive$: Subject<DbNotification<IDBNSource>[]> = new Subject<DbNotification<IDBNSource>[]>();
    private readonly _countUnread$: BehaviorSubject<number> = new BehaviorSubject<number>(0);

    public get notifications(): DbNotification<IDBNSource>[] {
        return this.__notifications;
    }

    public readonly dbNotificatinsRecive$: Observable<DbNotification<IDBNSource>[]> = 
        this._dbNotificatinsRecive$.pipe(tap(() => this.updateOutputMassOfNtfs()));

    public readonly countUnread$: Observable<number> = this._countUnread$.asObservable();

    public get countUnread(): number {
        return this._countUnread$.value;
    } 

    constructor(private _baseService: BaseService,
                private _userService: UserService,
                private _ntfService: NotificationService) {
        this.ser = apiConfig["notification"];

        this._ntfService.reciveNotification.subscribe((data) => {
            switch (data.type){
                case "dataBase":
                    this._countUnread$.next(this.countUnread + 1);
                    this._dbNotificatinsRecive$.next([this.localAddNotifications(<DataBaseSource>data.source)]);
                  break;
                case "onReadNtf":
                    this.readNtfs(<OnReadNtfSource>data.source)
                  break;
              }
        });

        this._userService.userObs.subscribe(x => {
            if (x){
                this._notifications = {};
                this.__notifications = [];
                if (x.login.countUnreadNotifications){
                    this._countUnread$.next(x.login.countUnreadNotifications);
                    x.login.countUnreadNotifications = 0;
                    this._userService.writeInStorage(x);
                }else 
                this.getCountUnread().subscribe(s => {
                    this._countUnread$.next(s);
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

    private sortDbNtifications(arr: DbNotification<IDBNSource>[]): DbNotification<IDBNSource>[]{
        return arr.sort((a: DbNotification<IDBNSource>, b: DbNotification<IDBNSource>) => {
            if (a.date > b.date) 
                return -1;
            if (a.date < b.date) 
                return 1;
            return 0;
        });
    }

    private updateOutputMassOfNtfs(){
        this.__notifications = this.sortDbNtifications(Object.values(this._notifications));
    }

    private readNtfs(read: OnReadNtfSource){
        const last : DbNotification<IDBNSource> = this._notifications[read.last];
        let count: number = read.count;
        Object.values(this._notifications).forEach((x, i) => {
            if (x.date < last.date || !x.id) return;
            x.read = true;
            --count;
        });
        this._countUnread$.next(this.countUnread - read.count);
    }

    private localAddNotifications(source: DataBaseSource): DbNotification<IDBNSource>{
        const id: string = source.id == null ? Guid.create().toString() : source.id;
        this._notifications[id] = new DbNotification<IDBNSource>(source);
        return this._notifications[id];
    }

    public readedNtfs(): Observable<any>{
        const ntfs: DbNotification<IDBNSource>[] = Object.values(this._notifications);
        const fullNtfs = this.sortDbNtifications(ntfs.filter((x) => !x.read && x.id));
        const noneNtfs = ntfs.filter((x) => !x.read && !x.id);

        const readed = noneNtfs.length + fullNtfs.length;

        if (noneNtfs.length > 0)
            noneNtfs.forEach((x) => x.read = true);
        
        let obs: Observable<any>;
        if (fullNtfs.length > 0)
            obs = this.read(fullNtfs[fullNtfs.length-1].id);
        else obs = of();
        
        return obs.pipe(
            tap(() => {
                if (readed > 0)
                    this._countUnread$.next(this.countUnread - readed); 
            })
        );
    }

    public getNtfs(): Observable<boolean> {
        return this.getn(this.getLastNtfId()).pipe(map(x => {
            this._dbNotificatinsRecive$.next(x.map(r => this.localAddNotifications(r)));
            return x.length < 20; 
        }));
    }

    public getLastNtfId(): string {
        const ntfs = this.notifications.filter(x => x.id);
        return ntfs.length > 0 ? ntfs[ntfs.length-1].id : null;
    }    

    public read(last: string = null): Observable<any> {
        return this._baseService.send<any>(this.ser, "read", null, { params: last == null ? null : {last: last} });
    }

    public getn(last: string = null): Observable<DataBaseSource[]> {
        return this._baseService.send<DataBaseSource[]>(this.ser, "get", null, { params: last == null ? null : {last: last} })
    }

    public getAndRead(last: string = null): Observable<DataBaseSource[]> {
        return this._baseService.send<DataBaseSource[]>(this.ser, "getandread", null, { params: last == null ? null : {last: last} });
    }

    public getCountUnread(): Observable<number> {
        return this._baseService.send<number>(this.ser, "getcountunread");
    }
}