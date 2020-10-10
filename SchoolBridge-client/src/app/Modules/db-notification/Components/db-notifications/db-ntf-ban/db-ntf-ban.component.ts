import { Component } from '@angular/core';
import { DbNtfComponent } from '../db-ntf-component.iterface';
import { IDBNSource } from '../../../Models/IDBN-source.interface';
import { DbNotification } from '../../../Services/db-notification.service';

export interface DBNBanSource extends IDBNSource{
    reason: string;
}

@Component({
    selector: "db-ban",
    styleUrls: ['../../base-db-ntf-component/base-db-ntf.component.css'],
    template: `<div class="top-text-heading" dbstring >ban</div>
                <p class="top-text-light" ><span dbstring >Reason</span>:{{model.source.reason}}</p>`

})
export class DbNtfBanComponent implements DbNtfComponent {
    public model: DbNotification<DBNBanSource>;
}
