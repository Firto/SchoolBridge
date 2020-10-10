import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminPanelRoutingModule } from './admin-panel-routing.module';
import { AdminPanelComponent } from './main/admin-panel.component';
import { UserPermissionModule } from '../user-permission/user-permission.module';
import { GlobalizationComponent } from './Components/globalization/globalization.component';
import { ReactiveFormsModule } from '@angular/forms';
import { GlobalizationModule } from 'src/app/Modules/globalization/globalization.module';
import { EditUsersComponent } from './Components/edit-users/edit-users.component';
import { UserRowComponent } from './Components/edit-users/Components/user-row/user-row.component';
import { NgxModalWindowModule } from 'ngx-modal-window';

@NgModule({
    declarations: [
        AdminPanelComponent,
        GlobalizationComponent,
        EditUsersComponent,
        UserRowComponent
    ],
    imports: [
        CommonModule,
        AdminPanelRoutingModule,
        UserPermissionModule,
        GlobalizationModule,
    ],
    providers: [

    ],
    exports: [
        AdminPanelComponent
    ]
})
export class AdminPanelModule  {}
