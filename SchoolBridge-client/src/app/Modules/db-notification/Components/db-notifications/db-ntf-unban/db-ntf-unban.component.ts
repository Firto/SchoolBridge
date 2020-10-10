import { Component } from '@angular/core';
import { DbNtfComponent } from '../db-ntf-component.iterface';
import { IDBNSource } from '../../../Models/IDBN-source.interface';
import { DbNotification } from '../../../Services/db-notification.service';

export interface DBNBanSource extends IDBNSource{
    reason: string;
}

@Component({
    selector: "db-unban",
    styleUrls: ['../../base-db-ntf-component/base-db-ntf.component.css'],
    template: `<div class="top-text-heading" dbstring >unban</div>`

})
export class DbNtfUnbanComponent implements DbNtfComponent {
    public model: DbNotification<DBNBanSource>;
}
