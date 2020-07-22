import { NgModule } from '@angular/core';
import { TimeAgoPipe } from 'src/app/PipeTransformers/time-ago.pipe';
import { DbNtfMessageComponent } from './Components/db-notifications/db-ntf-message/db-ntf-message.component';
import { CommonModule } from '@angular/common';
import { NgxMutationObserverDirective } from 'src/app/Directives/ngx-mutation-observer.directive';
import { SetDbNotificationsComponent } from './Components/set-db-notifications/set-db-notifications.component';
import { DbNotificationService } from './Services/db-notification.service';
import { DbNotificationsMapper } from './Mappers/db-notification.mapper';
import { DbNtfOnLoginComponent } from './Components/db-notifications/db-ntf-on-login/db-ntf-on-login.component';

@NgModule({
    declarations: [
        NgxMutationObserverDirective,
        SetDbNotificationsComponent,
        DbNtfMessageComponent,
        DbNtfOnLoginComponent,
        TimeAgoPipe
    ],
    imports: [
        CommonModule
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