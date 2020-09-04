import { NgModule } from '@angular/core';
import { DbNtfMessageComponent } from './Components/db-notifications/db-ntf-message/db-ntf-message.component';
import { CommonModule } from '@angular/common';
import { NgxMutationObserverDirective } from 'src/app/Directives/ngx-mutation-observer.directive';
import { SetDbNotificationsComponent } from './Components/set-db-notifications/set-db-notifications.component';
import { DbNotificationService } from './Services/db-notification.service';
import { DbNotificationsMapper } from './Mappers/db-notification.mapper';
import { DbNtfOnLoginComponent } from './Components/db-notifications/db-ntf-on-login/db-ntf-on-login.component';
import { DbNtfNewChatMessageComponent } from './Components/db-notifications/db-ntf-new-chat-message/db-ntf-new-chat-message.component';
import { TimeAgoPipeModule } from '../TimeAgoPipe/time-ago-pipe.module';

@NgModule({
    declarations: [
        NgxMutationObserverDirective,
        SetDbNotificationsComponent,
        DbNtfMessageComponent,
        DbNtfOnLoginComponent,
        DbNtfNewChatMessageComponent,
    ],
    imports: [
        CommonModule,
        TimeAgoPipeModule
    ],
    exports: [
        SetDbNotificationsComponent
    ]
})
export class DbNotificationModule  {
    static forRoot() {
        return {
            ngModule: DbNotificationModule,
            providers: [
                DbNotificationService,
                DbNotificationsMapper
            ],
        };
    }
} 