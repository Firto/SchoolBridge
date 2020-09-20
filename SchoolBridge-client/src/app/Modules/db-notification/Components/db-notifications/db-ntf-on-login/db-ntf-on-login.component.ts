import { Component } from '@angular/core';
import { DbNtfComponent } from '../db-ntf-component.iterface';
import { DataBaseSource } from 'src/app/Modules/notification/Models/NotificationSources/database-source';
import { Router } from '@angular/router';
import { DBNMessageSource } from '../db-ntf-message/db-ntf-message.component';
import { DbNotification } from '../../../Services/db-notification.service';

@Component({
    selector: "db-ntf-on-login",
    styleUrls: ['../../base-db-ntf-component/base-db-ntf.component.css'],
    template: `<div class="top-text-heading">{{model.source.message}}</div>
                <a class="top-text-light">if this not you, change your password!</a>`
    
})
export class DbNtfOnLoginComponent implements DbNtfComponent {
    public model: DbNotification<DBNMessageSource>;

    constructor (private router: Router){}

    onChangePassword(){

        
    }
}