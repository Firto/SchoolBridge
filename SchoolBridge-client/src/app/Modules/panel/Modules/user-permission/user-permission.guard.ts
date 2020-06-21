import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { UserPermissionService } from './user-permission.service';
import { ToastrService } from 'ngx-toastr';

export function userPermissionGuardFor(...permissions: string[]) {
    @Injectable({ providedIn: 'root' })
    class UserPermissionGuard {
        constructor(private userPermService: UserPermissionService,
                    private router: Router,
                    private toastrService: ToastrService) { } // uses the parent class instance actually, but could in theory take any other deps

        canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
            return this.canActivate(childRoute, state);
        }

        canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
            if (!this.userPermService.HasPermission(permissions)){
                this.toastrService.error("No permission to page!");
                this.router.navigateByUrl('/panel');
                return false;
            }
            return true;
        }
    }
    UserPermissionService.guards.push(UserPermissionGuard);
    return UserPermissionGuard;
  }