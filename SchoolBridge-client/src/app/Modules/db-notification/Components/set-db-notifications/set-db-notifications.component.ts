import { Component, ViewContainerRef, ViewChild, AfterViewInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { KeyedCollection } from 'src/app/Collections/keyed-collection';
import { DbNtfComponent } from '../db-notifications/db-ntf-component.iterface';
import { DbNotificationService } from '../../Services/db-notification.service';
import { DataBaseSource } from '../../../notification/Models/NotificationSources/database-source';
import { DbNotificationsMapper } from '../../Mappers/db-notification.mapper';
import { Guid } from "guid-typescript";
import { UserService } from 'src/app/Services/user.service';

@Component({
    selector: "set-db-notifications",
    styleUrls: ['./set-db-notifications.component.css'],
    templateUrl: './set-db-notifications.component.html'
})

export class SetDbNotificationsComponent implements AfterViewInit {
    @ViewChild('notificationContainer', { read: ViewContainerRef }) notifications: ViewContainerRef = null;
    public loader: boolean = false;

    private notificationList: KeyedCollection<string, DbNtfComponent> = new KeyedCollection<string, DbNtfComponent>();
    private end: boolean = false; 

    constructor(public ntfService: DbNotificationService,
                private ntfMapper: DbNotificationsMapper) {

        this.ntfService.onReciveDbNotification.subscribe((data) => this.onReciveDbNotification(data.key, data.value, data.reverse));

        this.ntfService.notificationList.items.forEach(element => {
            this.onReciveDbNotification(element.key, element.value);
        });
    }

    ngAfterViewInit(): void {
        this.ntfService.localUpdateCountUnreadNtfs();
    }
    
    public clear() {
        if (this.notifications != null)
            this.notifications.clear();
        this.notificationList.clear();
        this.end = false;
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
                }else
                    this.ntfService.readedNtfs();
            }
        });
    }

    public getNtfs(): void {
        this.loader = true;
        this.ntfService.getNtfs().subscribe(x => {
            this.end = x;
            this.loader = false;
        });
    }

    private onReciveDbNotification(key: string, source: DataBaseSource, reverse: boolean = true): void {
        const mon: DbNtfComponent =  this.notifications.createComponent(this.ntfMapper.map(source.type), reverse ? 0 : null).instance;
        mon.source = JSON.parse(atob(source.base64Sourse));
        mon.baseSource = source;
        if (reverse)
            this.notificationList.addOrUpdateShift(key, mon);
        else  this.notificationList.addOrUpdate(key, mon);
    }
}