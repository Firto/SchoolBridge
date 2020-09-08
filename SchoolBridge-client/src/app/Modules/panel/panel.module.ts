import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PanelComponent } from './main/panel.component';
import { DbNotificationModule } from '../db-notification/db-notification.module';
import { PanelNavbarComponent } from './Components/panel-navbar/panel-navbar.component';
import { UserPermissionModule } from './Modules/user-permission/user-permission.module';
import { PanelRoutingModule } from './panel-routing.module';
import { SelectPanelComponent } from './Components/select-panel/select-panel.component';
import { SettingsComponent } from './Components/settings/settings.component';
import { GlobalizationModule } from '../globalization/globalization.module';
import { ProfileService } from './Services/profile.service';
import { UsersService } from './Services/users.service';
import { OnlineService } from './Services/online.service';

@NgModule({
    declarations: [
        PanelComponent,
        PanelNavbarComponent,
        SelectPanelComponent,
        SettingsComponent
    ],
    imports: [
        CommonModule,
        ReactiveFormsModule,
        FormsModule,
        DbNotificationModule,
        UserPermissionModule.forRoot(),
        PanelRoutingModule,
        GlobalizationModule
    ],
    providers: [
        ProfileService,
        OnlineService,
        UsersService,
    ],
    exports: [
        PanelComponent
    ]
})
export class PanelModule  {} 