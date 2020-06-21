import { Injectable } from '@angular/core';
import { UserService } from '../../../../Services/user.service';
import { Subject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class UserPermissionService {
    public static guards = [ ]; 
    public onUpdatePermissions: Subject<string[]> = new Subject<string[]>();

    constructor(private userService: UserService) {
        this.userService.user.subscribe(x => {
            if (x != null)
                this.onUpdatePermissions.next(x.login.permissions);
            else this.onUpdatePermissions.next([]);
        });
    }

    public HasPermission(names: string[]): boolean {
        if (this.userService.user.value == null || 
            !names.every(el => this.userService.user.value.login.permissions.includes(el)))
            return false;
        return true;
    }
}