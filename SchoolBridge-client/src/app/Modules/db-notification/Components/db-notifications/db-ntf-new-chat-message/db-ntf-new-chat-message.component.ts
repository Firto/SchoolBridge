import { Component } from '@angular/core';
import { DbNtfComponent } from '../db-ntf-component.iterface';
import { DataBaseSource } from 'src/app/Modules/notification/Models/NotificationSources/database-source';
import { IDBNSource } from '../../../Models/IDBN-source.interface';
import { ShortUserModel } from 'src/app/Modules/panel/Models/short-user.model';
import { Observable } from 'rxjs';
import { UserModel } from 'src/app/Modules/panel/Models/user.model';
import { UsersService } from 'src/app/Modules/panel/Services/users.service';
import { apiConfig } from 'src/app/Const/api.config';
import { environment } from 'src/environments/environment';
import { User } from 'src/app/Modules/panel/Clases/user.class';

export interface DBNNewChatMessageSource extends IDBNSource{
    sender: ShortUserModel;
    type: string;
    date: Date 
    base64Source: string;
}

@Component({
    selector: "db-ntf-on-new-chat-message",
    styleUrls: ['../db-ntf.component.css', 'db-ntf-new-chat-message.component.css'],
    template: `<li [ngClass]="{'unread' : !baseSource.read}" class="top-text-block" [ngSwitch]="source.type" >
                   <div *ngSwitchCase="'text'" class="text-message">
                        <img src="{{(sender | async)?.photo}}" alt="avatar" />
                        <div class="about">
                            <div class="name">
                                {{(sender | async)?.login}}
                                <span *ngIf="((sender | async)?.onlineStatus | async) !== 2" class="status">
                                    <i class="fa fa-circle" 
                                    [ngClass]="{'online' : ((sender | async)?.onlineStatus | async) === 1,
                                                'offline' : ((sender | async)?.onlineStatus | async) === 0 }" ></i>
                                    online
                                </span>
                            </div>
                        </div>
                        <p class="top-text-light">new message: {{messageSource.text}}</p>
                        <p class="top-text-light">{{baseSource.date | timeAgo}}</p>
                    </div>
                    <div *ngSwitchDefault>

                    </div>
                </li>`
})

export class DbNtfNewChatMessageComponent implements DbNtfComponent {
    private _sender: Observable<User>;

    public baseSource: DataBaseSource;
    private _source: DBNNewChatMessageSource = null;


    get source(): DBNNewChatMessageSource {
        
        return this._source;
        
    }
    set source(value: DBNNewChatMessageSource) {
        this._source = value;
        this.messageSource = JSON.parse(atob(this._source.base64Source));
        this._sender = this._usersService.get(this._source.sender);
    }

    get sender() {
        return this._sender;
    }

    public messageSource: any;

    constructor (private _usersService: UsersService){
    }
}