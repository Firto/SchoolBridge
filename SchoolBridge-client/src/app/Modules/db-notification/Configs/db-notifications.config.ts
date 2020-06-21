import { DbNtfComponent } from '../Components/db-notifications/db-ntf-component.iterface';
import { Type } from '@angular/core';
import { DbNtfMessageComponent } from '../Components/db-notifications/db-ntf-message/db-ntf-message.component';
import { DbNtfOnLoginComponent } from '../Components/db-notifications/db-ntf-on-login/db-ntf-on-login.component';

export const dbNotificatonsConfig:  {[name: string]: Type<DbNtfComponent>} = {
    "test":  DbNtfMessageComponent,
    "onLogin":  DbNtfOnLoginComponent
}

