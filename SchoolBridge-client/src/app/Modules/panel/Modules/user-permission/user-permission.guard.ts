import { Injectable } from "@angular/core";
import {
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  Router,
  CanActivate,
  CanActivateChild,
  CanLoad,
  Route,
  UrlSegment,
} from "@angular/router";
import { Observable } from "rxjs";
import { GlobalizationStringService } from 'src/app/Modules/globalization/Services/globalization-string.service';
import { Toaster } from 'src/app/Modules/ngx-toast-notifications';
import { UserPermissionService } from "./user-permission.service";
//import { ToastrService } from 'ngx-toastr';

@Injectable()
export class UserPermissionGuard
  implements CanActivate, CanActivateChild, CanLoad {
  constructor(
    private _userPermService: UserPermissionService,
    private toastrService: Toaster,
    private _gbsService: GlobalizationStringService,
    private _router: Router
  ) {} // uses the parent class instance actually, but could in theory take any other deps
  canLoad(
    route: Route,
    segments: UrlSegment[]
  ): boolean | Observable<boolean> | Promise<boolean> {

    return this.canActivate(<ActivatedRouteSnapshot>route, null);
  }

  canActivateChild(
    childRoute: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ) {
    return this.canActivate(childRoute, state);
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    if (
      route.data.perms &&
      !this._userPermService.HasPermission(route.data.perms)
    ) {
      this.toastrService.open(this._gbsService.getStringObs('no-perm-page'), {type: 'danger'});
      this._router.navigateByUrl("/panel");
      return false;
    }
    return true;
  }
}
