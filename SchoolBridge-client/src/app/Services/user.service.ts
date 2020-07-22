import { BehaviorSubject, Observable } from 'rxjs';
import { User } from '../Models/user.model';
import { Injectable, Injector } from '@angular/core';
import { ActivatedRoute, RouterStateSnapshot } from '@angular/router';
import { NotificationService } from '../Modules/notification/Services/notification.service';
import { Loginned } from '../Models/loginned.model';
import { LoginnedTokens } from '../Models/loginned-tokens';
import { CryptService } from './crypt.service';
import { DeviceUUIDService } from './device-uuid.service';

@Injectable({ providedIn: 'root' })
export class UserService {
    private _user: BehaviorSubject<User> = new BehaviorSubject<User>(null);

    public user: Observable<User> = this._user.asObservable();
    public getUser(): User {return this._user.getValue()};

    public uuid: string;

    constructor(private _crypt: CryptService,
                private _route: ActivatedRoute,
                private _injector: Injector,
                private _notificationService: NotificationService,
                uuidService: DeviceUUIDService) {
        this.uuid = uuidService.get(); 

        if (localStorage.getItem('user')){
            try {
                this._user.next(JSON.parse(this._crypt.decode(localStorage.getItem('user'), this.uuid)));
                this._notificationService.subscribe(this._user.value.login.tokens.token);
            }
            catch{
                this.localLogout();
            }
        }
    }

    

    forceRunAuthGuard(): void {
        console.log(this._route);
        if (this._route.children.length && this._route.children['0'].snapshot._routeConfig.canActivate) {
            const curr__route = this._route.children[ '0' ];
            const AuthGuard = curr__route.snapshot._routeConfig.canActivate[ '0' ];
            const authGuard = this._injector.get(AuthGuard);
            const _routerStateSnapshot: RouterStateSnapshot = Object.assign({}, curr__route.snapshot, {url: "/"+curr__route.snapshot.url[0]});
            authGuard.canActivate(curr__route.snapshot, _routerStateSnapshot);
        }
    }

    // local

    localLogout(): void {
        localStorage.removeItem('user');
        this._user.next(null);
        this._notificationService.unsubscribe();
        //this.forceRunAuthGuard();
    }

    localLogin(login: Loginned): void {
        const user = new User();
        user.login = login;
        this._user.next(user);
        localStorage.setItem('user', this._crypt.encode(JSON.stringify(this.getUser()), this.uuid));
        this._notificationService.subscribe(login.tokens.token);
    }

    localSetLoginTokens(tokens: LoginnedTokens): void {
        const user = this.getUser();
        user.login.tokens = tokens;
        this._user.next(user);
        localStorage.setItem('user', this._crypt.encode(JSON.stringify(this.getUser()), this.uuid));
        this._notificationService.subscribe(user.login.tokens.token);
    }
}