import {Pipe, PipeTransform, ChangeDetectorRef} from "@angular/core";
import { UserPermissionService } from './user-permission.service';

@Pipe({
	name:'userPerm',
	pure:false
})
export class UserPermissionPipe implements PipeTransform {

    constructor(private userPermissionService: UserPermissionService,
                changeDetectorRef: ChangeDetectorRef) {
        userPermissionService.onUpdatePermissions.subscribe((x) => {
            changeDetectorRef.markForCheck();
        });
    }
    
	transform(value:string): boolean {
		return this.userPermissionService.HasPermission([value]);
    }
    
}