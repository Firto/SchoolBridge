import { Component, ViewContainerRef, ViewChild, AfterViewInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { KeyedCollection } from 'src/app/Collections/keyed-collection';
import { DbNtfComponent } from '../db-notifications/db-ntf-component.iterface';
import { DbNotificationService } from '../../Services/db-notification.service';
import { DataBaseSource } from '../../../../Models/NotificationSources/database-source';
import { DbNotificationsMapper } from '../../Mappers/db-notification.mapper';

@Component({
    selector: "set-db-notifications",
    styleUrls: ['./set-db-notifications.component.css'],
    templateUrl: './set-db-notifications.component.html'
})

export class SetDbNotificationsComponent implements AfterViewInit {
    @ViewChild('notificationContainer', { read: ViewContainerRef }) notifications: ViewContainerRef = null;
    public countUnredNtfs: BehaviorSubject<number> = new BehaviorSubject<number>(0);
    public loader: boolean = false;

    private notificationList: KeyedCollection<string, DbNtfComponent> = new KeyedCollection<string, DbNtfComponent>();
    private end: boolean = false;

    constructor(private ntfService: DbNotificationService,
                private ntfMapper: DbNotificationsMapper) {

        this.ntfService.onReciveDbNotification.subscribe((data) => this.onReciveDbNotification(data));

        this.ntfService.onReciveOnReadDbNotification.subscribe((data) => {
            for (let ind = this.notificationList.getIndex(data.last); ind >= 0; ind--) 
                this.notificationList.items[ind].value.baseSource.read = true;
            this.countUnredNtfs.next(this.countUnredNtfs.value - data.count);
        });
    }

    ngAfterViewInit(): void {
        this.getCountNtfs();
    }
    
    public clear() {
        if (this.notifications != null)
            this.notifications.clear();
        this.notificationList.clear();
        this.end = false;
    }

    public getCountNtfs() {
        this.ntfService.getCountUnread().subscribe((x) => {
            if (x.ok)
                this.countUnredNtfs.next(x.result);
        });
    }

    public onNtfScroll(event: Event) {
        const el: any = event.target;
        if (el.scrollTop + el.clientHeight + 20 > el.scrollHeight && !this.loader && !this.end)
            this.getNtfs();
    }

    public onNtfMenuChange(mutations: MutationRecord[]): void {
        mutations.forEach((mutation: MutationRecord) => {
            if (mutation.type == "attributes") {
                if ((<HTMLElement>mutation.target).attributes.getNamedItem("aria-expanded").value === "true") {
                    if (!this.end && !this.loader)
                        this.getNtfs();
                }
                else if (this.notificationList.items.some((x) => x.value.baseSource.read == false))
                    this.ntfService.read(this.getLastNtfId()).subscribe();
            }
        });
    }

    public getNtfs(): void {
        this.loader = true;
        this.ntfService.getn(this.getLastNtfId()).subscribe((data) => {
            if (data.ok) {
                const res: DataBaseSource[] = <DataBaseSource[]>data.result;
                if (res.length > 0) {
                    res.forEach(baseSource => this.onReciveDbNotification(baseSource, false));
                    if (res.length < 20)
                        this.end = true;
                }
                else this.end = true;
            }
            this.loader = false;
        });
    }

    private onReciveDbNotification(baseSource: DataBaseSource, reverse:boolean = true): void {
        const mon: DbNtfComponent =  this.notifications.createComponent(this.ntfMapper.map(baseSource.type), reverse ? 0 : null).instance;
        mon.source = JSON.parse(atob(baseSource.base64Sourse));
        mon.baseSource = baseSource;
        if (reverse) 
            this.notificationList.addOrUpdateShift(mon.baseSource.id, mon);
        else this.notificationList.addOrUpdate(mon.baseSource.id, mon);
    }


    private getLastNtfId(): string {
        return this.notificationList.length() > 0 ? this.notificationList.items[this.notificationList.length() - 1].key : null;
    }    
}