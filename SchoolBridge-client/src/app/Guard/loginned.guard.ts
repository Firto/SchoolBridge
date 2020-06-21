import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router, CanActivateChild } from '@angular/router';
import { UserService } from '../Services/user.service';
import { ToastrService } from 'ngx-toastr';

@Injectable({ providedIn: 'root' })
export class LoginnedGuard implements CanActivate, CanActivateChild {
    constructor(private userService: UserService,
                private toastrService: ToastrService,
                private router: Router) { }

    canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        return this.canActivate(childRoute, state);
    }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        if (this.userService.user.value) 
            return true;
        this.toastrService.error("No permission to page!");
        this.router.navigateByUrl('/start?returnUrl=' + encodeURI(window.location.pathname));
        return false;
    }
}