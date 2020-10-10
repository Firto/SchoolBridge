import { DbNtfComponent } from '../Components/db-notifications/db-ntf-component.iterface';
import { Type } from '@angular/core';
import { DbNtfMessageComponent } from '../Components/db-notifications/db-ntf-message/db-ntf-message.component';
import { DbNtfOnLoginComponent } from '../Components/db-notifications/db-ntf-on-login/db-ntf-on-login.component';
import { DbNtfNewChatMessageComponent } from '../Components/db-notifications/db-ntf-new-chat-message/db-ntf-new-chat-message.component';
import { DbNtfBanComponent } from '../Components/db-notifications/db-ntf-ban/db-ntf-ban.component';
import { DbNtfUnbanComponent } from '../Components/db-notifications/db-ntf-unban/db-ntf-unban.component';

export const dbNotificatonsConfig:  Record<string, Type<DbNtfComponent>> = {
    "test":  DbNtfMessageComponent,
    "onLogin":  DbNtfOnLoginComponent,
    "newChatMessage":  DbNtfNewChatMessageComponent,
    "Ban":  DbNtfBanComponent,
    "Unban":  DbNtfUnbanComponent
}

