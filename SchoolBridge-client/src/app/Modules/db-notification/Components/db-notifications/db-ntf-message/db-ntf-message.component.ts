import { Component } from '@angular/core';
import { DbNtfComponent } from '../db-ntf-component.iterface';
import { DataBaseSource } from 'src/app/Modules/notification/Models/NotificationSources/database-source';
import { IDBNSource } from '../../../Models/IDBN-source.interface';
import { DbNotification } from '../../../Services/db-notification.service';

export interface DBNMessageSource extends IDBNSource{
    message: string;
}

@Component({
    selector: "db-ntf-message",
    styleUrls: ['../../base-db-ntf-component/base-db-ntf.component.css'],
    template: `<div class="top-text-heading">{{model.source.message}}</div>`
    
})
export class DbNtfMessageComponent implements DbNtfComponent {
    public model: DbNotification<DBNMessageSource>;
}