import { ChangeDetectorRef, Component } from '@angular/core';
import { DbNtfComponent } from '../db-ntf-component.iterface';
import { DataBaseSource } from 'src/app/Modules/notification/Models/NotificationSources/database-source';
import { IDBNSource } from '../../../Models/IDBN-source.interface';
import { ShortUserModel } from 'src/app/Modules/panel/Models/short-user.model';
import { Observable, Subject } from 'rxjs';
import { UserModel } from 'src/app/Modules/panel/Models/user.model';
import { UsersService } from 'src/app/Modules/panel/Services/users.service';
import { apiConfig } from 'src/app/Const/api.config';
import { environment } from 'src/environments/environment';
import { ShortUser, User } from 'src/app/Modules/panel/Clases/user.class';
//import { Globalization } from 'src/app/Modules/globalization/Decorators/backend-strings.decorator';
import { GlobalizationService } from 'src/app/Modules/globalization/Services/globalization.service';
import { fromBinary } from 'src/app/Modules/binary/from-binary.func';
import { MdGlobalization } from 'src/app/Modules/globalization/Services/md-globalization.service';
import { OnUnsubscribe } from 'src/app/Services/super.controller';
import { debounceTime, mergeMap, takeUntil } from 'rxjs/operators';
import { merge } from 'jquery';
import { DbNotification } from '../../../Services/db-notification.service';
import { observed } from 'src/app/Decorators/observed.decorator';

export interface DBNNewChatMessageSource extends IDBNSource{
    sender: ShortUserModel;
    type: string;
    date: Date
    base64Source: string;
}

@Component({
    selector: "db-ntf-on-new-chat-message",
    styleUrls: ['../../base-db-ntf-component/base-db-ntf.component.css', 'db-ntf-new-chat-message.component.css'],
    template: `<div [ngSwitch]="model.source.type" >
                   <div *ngSwitchCase="'text'" class="text-message">
                        <div *ngIf="sender.user; else elseBlock">
                            <img src="{{sender.user.photo}}" alt="avatar" />
                            <div class="about">
                                <div class="name">
                                    {{sender.user.login}}
                                    <span *ngIf="sender.user.onlineStatus !== 2" class="status">
                                        <i class="fa fa-circle"
                                        [ngClass]="{'online' : sender.user.onlineStatus === 1,
                                                    'offline' : sender.user.onlineStatus === 0 }" ></i>
                                        online
                                    </span>
                                </div>
                            </div>
                        </div>
                        <ng-template #elseBlock>
                            <div class="loader-topbar"></div>
                        </ng-template>
                        <p class="top-text-light" ><span dbstring >new-msg</span>: {{messageSource.text}}</p>
                    </div>
                    <div *ngSwitchDefault>

                    </div>
                </div>`,
    providers: MdGlobalization("ch-msg")
})
//@Globalization('cm-db-ntf-on-new-chat-msg', [])
export class DbNtfNewChatMessageComponent extends OnUnsubscribe implements DbNtfComponent  {
    private _model: DbNotification<DBNNewChatMessageSource>;
    private _sender: ShortUser = null;

    set model(value: DbNotification<DBNNewChatMessageSource>) {
        this._model = value;
        this.messageSource = JSON.parse(fromBinary(this._model.source.base64Source));
        this._sender = this._usersService.get(this._model.source.sender);
        this._sender.userObs.pipe(takeUntil(this._destroy)).subscribe(x => {
            if (!x) return;

            x.onlineStatusObs.pipe(takeUntil(this._destroy)).subscribe(x => {
              this._sb.next();
            });
        });
    }
    get model(): DbNotification<DBNNewChatMessageSource> {
        return this._model;
    }

    get sender() {
        return this._sender;
    }

    public messageSource: any;

    protected _sb: Subject<unknown> = new Subject();
    @observed() public onChanged: Observable<unknown> = this._sb.pipe(debounceTime(1000));

    constructor (private _usersService: UsersService){
        super();
    }
}
