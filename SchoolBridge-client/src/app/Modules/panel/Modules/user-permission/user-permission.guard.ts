import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { UserPermissionService } from './user-permission.service';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class UserPermissionGuard {

    constructor(private _userPermService: UserPermissionService,
                private _router: Router) { } // uses the parent class instance actually, but could in theory take any other deps

    canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        return this.canActivate(childRoute, state);
    }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        if (route.data.perms && !this._userPermService.HasPermission(route.data.perms)){
            //this._toastrService.error("No permission to page!");
            this._router.navigateByUrl('/panel');
            return false;
        }
        return true;
    }
}
