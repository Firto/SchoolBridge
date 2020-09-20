import { Component, ViewContainerRef, ViewChild, AfterViewInit, ChangeDetectorRef, OnInit } from '@angular/core';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { KeyedCollection } from 'src/app/Collections/keyed-collection';
import { DbNtfComponent } from '../db-notifications/db-ntf-component.iterface';
import { DbNotification, DbNotificationService } from '../../Services/db-notification.service';
import { DataBaseSource } from '../../../notification/Models/NotificationSources/database-source';
import { DbNotificationsMapper } from '../../Mappers/db-notification.mapper';
import { Guid } from "guid-typescript";
import { UserService } from 'src/app/Services/user.service';
import { fromBinary } from 'src/app/Modules/binary/from-binary.func';
import { MdGlobalization } from 'src/app/Modules/globalization/Services/md-globalization.service';
import { observed } from 'src/app/Decorators/observed.decorator';
import { IDBNSource } from '../../Models/IDBN-source.interface';
import { debounceTime, takeUntil } from 'rxjs/operators';
import { OnUnsubscribe } from 'src/app/Services/super.controller';
import { markDirty } from 'src/app/Helpers/mark-dirty.func';

@Component({
    selector: "set-db-notifications",
    styleUrls: ['./set-db-notifications.component.css'],
    templateUrl: './set-db-notifications.component.html',
    providers: MdGlobalization("ntf")
})
export class SetDbNotificationsComponent extends OnUnsubscribe implements OnInit {
    public loader: boolean = false;
    private _onCh: Subject<any> = new Subject<any>();
    @observed() public onCh: Observable<any> = this._onCh.pipe(debounceTime(100));
     
    private end: boolean = false; 

    constructor(public ntfService: DbNotificationService) {
        super();
    }

    public ngOnInit(){
        this.ntfService.dbNotificatinsRecive$.pipe(takeUntil(this._destroy)).subscribe(x => {
            this.loader = false;
            this._onCh.next();
        });
        this.ntfService.countUnread$.pipe(takeUntil(this._destroy)).subscribe(x => {
            console.log(x);
            this._onCh.next();
        });
    }

    public onNtfScroll(event: Event) {
        const el: any = event.target;
        if (el.scrollTop + el.clientHeight + 20 > el.scrollHeight && !this.loader && !this.end)
            this.getNtfs();
    }

    public onChange(){
        this._onCh.next();
    }

    public onNtfMenuChange(mutations: MutationRecord[]): void {
        mutations.forEach((mutation: MutationRecord) => {
            if (mutation.type == "attributes") {
                if ((<HTMLElement>mutation.target).attributes.getNamedItem("aria-expanded").value === "true") {
                    if (!this.end && !this.loader){
                        this.getNtfs();
                    }
                }else
                    this.ntfService.readedNtfs().subscribe();
            }
        });
    }

    public getNtfs(): void {
        this.loader = true;
        markDirty(this);
        this.ntfService.getNtfs()
        .subscribe(
            x => this.end = x,
            ()=> {this.loader = false; markDirty(this);}
        );
    }
}