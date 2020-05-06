import { BehaviorSubject, Observable } from 'rxjs';
import { User } from '../Models/user.model';
import { Injectable, Injector } from '@angular/core';
import { DeviceUUIDService } from '../Helpers/device-uuid.service';
import { ActivatedRoute, RouterStateSnapshot } from '@angular/router';
import { CryptService } from '../Helpers/crypt.service';
import { NotificationService } from './notification.service';
import { Loginned } from '../Models/loginned.model';
import { LoginnedTokens } from '../Models/loginned-tokens';

@Injectable({ providedIn: 'root' })
export class UserService {
    public user: BehaviorSubject<User> = new BehaviorSubject<User>(null);

    public uuid: string;

    constructor(private crypt: CryptService,
                private route: ActivatedRoute,
                private injector: Injector,
                private uuidService: DeviceUUIDService,
                private notificationService: NotificationService) {
        this.uuid = uuidService.get(); 

        if (localStorage.getItem('user')){
            try {
                this.user.next(JSON.parse(this.crypt.decode(localStorage.getItem('user'), this.uuid)));
                this.notificationService.subscribe(this.user.value.login.tokens.token);
            }
            catch{
                this.localLogout();
            }
        }
    }

    forceRunAuthGuard(): void {
        if (this.route.root.children.length && this.route.root.children['0'].snapshot.routeConfig.canActivate) {
          const curr_route = this.route.root.children[ '0' ];
          const AuthGuard = curr_route.snapshot.routeConfig.canActivate[ '0' ];
          const authGuard = this.injector.get(AuthGuard);
          const routerStateSnapshot: RouterStateSnapshot = Object.assign({}, curr_route.snapshot, {url: "/"+curr_route.snapshot.url[0]});
          authGuard.canActivate(curr_route.snapshot, routerStateSnapshot);
        }
    }

    // local

    localLogout(): void {
        localStorage.removeItem('user');
        this.user.next(null);
        this.notificationService.unsubscribe();
        this.forceRunAuthGuard();
    }

    localLogin(login: Loginned): void {
        const user = new User();
        user.login = login;
        this.user.next(user);
        localStorage.setItem('user', this.crypt.encode(JSON.stringify(this.user.value), this.uuid));
        this.notificationService.subscribe(login.tokens.token);
    }

    localSetLoginTokens(tokens: LoginnedTokens): void {
        const user = this.user.value;
        user.login.tokens = tokens;
        this.user.next(user);
        localStorage.setItem('user', this.crypt.encode(JSON.stringify(this.user.value), this.uuid));
        this.notificationService.subscribe(user.login.tokens.token);
    }
}