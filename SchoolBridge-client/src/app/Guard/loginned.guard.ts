import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router, CanActivateChild } from '@angular/router';
import { UserService } from '../Services/user.service';
import { ToastrService } from 'ngx-toastr';
import { GuardService } from '../Services/guard.service';

@Injectable({ providedIn: 'root' })
export class LoginnedGuard implements CanActivate, CanActivateChild {
    constructor(private userService: UserService,
                /*private toastrService: ToastrService,*/
                private router: Router,
                private _guardService: GuardService) { }

    canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        return this.canActivate(childRoute, state);
    }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        if (!this._guardService.getState())
            return true;
        if (this.userService.user)  
            return true;
        //this.toastrService.error("No permission to page!");
        this.router.navigateByUrl('/start?returnUrl=' + encodeURI(window.location.pathname));
        return false;
    }
}