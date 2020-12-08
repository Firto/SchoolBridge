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
import { SubjectPanelComponent } from './Components/subject-panel/subject-panel.component';
import { SubjectService } from './Services/subject.service';

@NgModule({
    declarations: [
        PanelComponent,
        PanelNavbarComponent,
        SelectPanelComponent,
        SettingsComponent,
        SubjectPanelComponent
    ],
    providers: [
        ProfileService,
        OnlineService,
        UsersService,
        SubjectService
    ],
    imports: [
        CommonModule,
        PanelRoutingModule,
        GlobalizationModule,
        DbNotificationModule,
        UserPermissionModule.forRoot(),
    ],
    exports: [
        PanelComponent
    ]
})
export class PanelModule  {}
