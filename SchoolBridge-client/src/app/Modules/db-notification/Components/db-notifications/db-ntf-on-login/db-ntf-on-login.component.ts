import { Component } from '@angular/core';
import { DbNtfComponent } from '../db-ntf-component.iterface';
import { DataBaseSource } from 'src/app/Modules/notification/Models/NotificationSources/database-source';
import { Router } from '@angular/router';
import { DBNMessageSource } from '../db-ntf-message/db-ntf-message.component';

@Component({
    selector: "db-ntf-on-login",
    styleUrls: ['../db-ntf.component.css'],
    template: `<li [ngClass]="{'unread' : !baseSource.read}" class="top-text-block" >
                    <div class="top-text-heading">{{source.message}}</div>
                    <a class="top-text-light">if this not you, change your password!</a>
                    <div class="top-text-light">{{baseSource.date | timeAgo}}</div>
                </li>`
    
})

export class DbNtfOnLoginComponent implements DbNtfComponent {
    public baseSource: DataBaseSource;
    public source: DBNMessageSource;

    constructor (private router: Router){}

    onChangePassword(){

        
    }
}