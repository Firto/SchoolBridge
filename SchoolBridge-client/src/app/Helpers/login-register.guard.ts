import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { UserService } from '../Services/user.service';

@Injectable({ providedIn: 'root' })
export class LoginRegisterGuard implements CanActivate {
    constructor(private userService: UserService,
                private router: Router) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        if (!this.userService.user.value) 
            return true;
        this.router.navigate(['/news']);
        return false;
    }
}