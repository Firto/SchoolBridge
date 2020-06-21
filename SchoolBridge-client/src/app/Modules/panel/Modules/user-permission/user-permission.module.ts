import { NgModule } from '@angular/core';
import { UserPermissionPipe } from './user-permission.pipe';
import { UserPermissionService } from './user-permission.service';

@NgModule({
    declarations: [
        UserPermissionPipe
    ],
    imports: [
        
    ],
    providers: [
        UserPermissionService
    ],
    exports: [
        UserPermissionPipe
    ]
})
export class UserPermissionModule  {}