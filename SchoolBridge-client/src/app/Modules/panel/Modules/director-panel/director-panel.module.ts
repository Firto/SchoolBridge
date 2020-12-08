import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DirectorPanelRoutingModule } from './director-panel-routing.module';
import { UserPermissionModule } from '../user-permission/user-permission.module';
import { ReactiveFormsModule } from '@angular/forms';
import { DirectorPanelComponent } from './main/director-panel.component';
import { SetSchoolComponent } from './Components/set-school/set-school.component';

@NgModule({
    declarations: [,
        DirectorPanelComponent,
        SetSchoolComponent
    ],
    imports: [
        CommonModule,
        DirectorPanelRoutingModule,
        UserPermissionModule
    ],
    providers: [
        
    ],
    exports: [
        DirectorPanelComponent
    ]
})
export class DirectorPanelModule  {} 