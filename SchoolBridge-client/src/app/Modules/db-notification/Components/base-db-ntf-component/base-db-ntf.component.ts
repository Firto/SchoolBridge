import { Component, ViewContainerRef, ViewChild, AfterViewInit, ChangeDetectorRef, Input, ComponentFactoryResolver, ComponentFactory, ComponentRef, OnInit, AfterContentInit, Output, EventEmitter } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
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
import { detectChanges, markDirty } from 'src/app/Helpers/mark-dirty.func';

@Component({
    selector: "base-db-ntf",
    styleUrls: ['./base-db-ntf.component.css'],
    template: `
        <li [ngClass]="{'unread' : !ntf.read}" class="top-text-block" >
            <ng-template #ntfContainer></ng-template>
            <ng-template *ngIf="!componentRef">
                <div class="loader-topbar"></div>
            </ng-template>
            <p class="top-text-light" [timeAgo]="ntf.date" ></p>  
        </li>
    `
})
export class BaseDbNtfComponent implements AfterViewInit {
    @ViewChild('ntfContainer', { read: ViewContainerRef }) public container;
    @Input() public ntf: DbNotification<IDBNSource>;
    @Output() public onChange: EventEmitter<any> = new EventEmitter<any>();
    public componentRef: ComponentRef<DbNtfComponent> = null;

    constructor(private _mapper: DbNotificationsMapper) {}

    createComponent() {
        const bb = this.container.createComponent(this._mapper.map(this.ntf.type));
        bb.instance.model = this.ntf;
        this.componentRef = bb;
        this.onChange.emit(null); 
    }
      
    ngAfterViewInit(){
        this.createComponent();
    }

    ngOnDestroy() {
        this.componentRef.destroy();    
    }
}