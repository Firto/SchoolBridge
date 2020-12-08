import { NgModule } from '@angular/core';
import { UserPermissionGuard } from './user-permission.guard';
import { UserPermissionPipe } from './user-permission.pipe';
import { UserPermissionService } from './user-permission.service';

@NgModule({
    declarations: [
        UserPermissionPipe
    ],
    imports: [
        
    ],
    providers: [
       
    ],
    exports: [
        UserPermissionPipe
    ]
})
export class UserPermissionModule  {
    static forRoot() {
        return {
            ngModule: UserPermissionModule,
            providers: [
                UserPermissionService,
                UserPermissionGuard
            ]
        };
    }
}