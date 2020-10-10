import { Injectable } from "@angular/core";
import {
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  Router,
  CanActivateChild, CanLoad, Route
} from "@angular/router";
import { UserService } from "../Services/user.service";
//import { ToastrService } from 'ngx-toastr';
import { GuardService } from "../Services/guard.service";
import { Toaster } from "../Modules/ngx-toast-notifications";
import { GlobalizationStringService } from "../Modules/globalization/Services/globalization-string.service";

@Injectable({ providedIn: "root" })
export class LoginRegisterGuard implements CanActivate, CanActivateChild, CanLoad {
  constructor(
    private userService: UserService,
    private toastrService: Toaster,
    private router: Router,
    private _guardService: GuardService,
    private _gbsService: GlobalizationStringService
  ) {}

  canActivateChild(
    childRoute: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ) {
    return this.canActivate(childRoute, state);
  }

  canLoad(route: Route): boolean {
    return this.canActivate(null, null);
  }s

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    if (!this._guardService.getState()) return true;
    if (!this.userService.user) return true;

    this.toastrService.open(this._gbsService.getStringObs("no-perm-page"), {type: 'danger'});
    this.router.navigateByUrl("/panel");
    return false;
  }
}
