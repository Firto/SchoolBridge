import { Component } from '@angular/core';
import { DBNMessageSource } from 'src/app/Modules/db-notification/Sources/DataBaseSources/DBN-message-source';
import { DbNtfComponent } from '../db-ntf-component.iterface';
import { DataBaseSource } from 'src/app/Modules/db-notification/Sources/database-source';
import { Router } from '@angular/router';

@Component({
    selector: "db-ntf-on-login",
    styleUrls: ['../db-ntf.component.css'],
    template: `<li class="top-text-block" >
                    <div class="top-text-heading">{{source.message}}</div>
                    <a routerLink="/profile" (click)="onChangePassword()" class="top-text-light">if this not you, change your password!</a>
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