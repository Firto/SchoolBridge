import { NgModule } from '@angular/core';
import { DbNtfMessageComponent } from './Components/db-notifications/db-ntf-message/db-ntf-message.component';
import { CommonModule } from '@angular/common';
import { NgxMutationObserverDirective } from 'src/app/Directives/ngx-mutation-observer.directive';
import { SetDbNotificationsComponent } from './Components/set-db-notifications/set-db-notifications.component';
import { DbNotificationService } from './Services/db-notification.service';
import { DbNotificationsMapper } from './Mappers/db-notification.mapper';
import { DbNtfOnLoginComponent } from './Components/db-notifications/db-ntf-on-login/db-ntf-on-login.component';
import { DbNtfNewChatMessageComponent } from './Components/db-notifications/db-ntf-new-chat-message/db-ntf-new-chat-message.component';
import { GlobalizationModule } from '../globalization/globalization.module';
import { TimeAgoDirectiveModule } from '../TimeAgoPipe/time-ago-directive.module';
import { BaseDbNtfComponent } from './Components/base-db-ntf-component/base-db-ntf.component';
import { DbNtfBanComponent } from './Components/db-notifications/db-ntf-ban/db-ntf-ban.component';
import { DbNtfUnbanComponent } from './Components/db-notifications/db-ntf-unban/db-ntf-unban.component';

@NgModule({
    declarations: [
        NgxMutationObserverDirective,
        BaseDbNtfComponent,
        SetDbNotificationsComponent,
        DbNtfMessageComponent,
        DbNtfOnLoginComponent,
        DbNtfNewChatMessageComponent,
        DbNtfBanComponent,
        DbNtfUnbanComponent
    ],
    imports: [
        CommonModule,
        TimeAgoDirectiveModule,
        GlobalizationModule
    ],
    exports: [
        SetDbNotificationsComponent
    ]
})
export class DbNotificationModule {}
