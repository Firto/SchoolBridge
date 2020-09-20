import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminPanelRoutingModule } from './admin-panel-routing.module';
import { AdminPanelComponent } from './main/admin-panel.component';
import { UserPermissionModule } from '../user-permission/user-permission.module';
import { HomeComponent } from './Components/home/home.component';
import { GlobalizationComponent } from './Components/globalization/globalization.component';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
    declarations: [
        AdminPanelComponent,
        HomeComponent,
        GlobalizationComponent
    ],
    imports: [
        CommonModule,
        AdminPanelRoutingModule,
        UserPermissionModule
    ],
    providers: [
        
    ],
    exports: [
        AdminPanelComponent
    ]
})
export class AdminPanelModule  {} 