import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { User } from '../Models/user.model';
import { Injectable, Injector } from '@angular/core';
import { ActivatedRoute, RouterStateSnapshot } from '@angular/router';
import { NotificationService } from '../Modules/notification/Services/notification.service';
import { Loginned } from '../Models/loginned.model';
import { LoginnedTokens } from '../Models/loginned-tokens';
import { CryptService } from './crypt.service';
import { DeviceUUIDService } from './device-uuid.service';
import { ClientConnectionService } from './client-connection.service';
import { MyLocalStorageService } from './my-local-storage.service';

@Injectable()
export class UserService {
    private _user: BehaviorSubject<User> = new BehaviorSubject<User>(null);
    public get userObs(): Observable<User> {return this._user;};
    
    public get user(): User {return this._user.value};
    public set user(usr: User) {
        this._localStorage.write("user", usr);
        this._user.next(usr);
    };
    constructor(private _clientConnectionService: ClientConnectionService,
                private _localStorage: MyLocalStorageService,
                private _uuidService: DeviceUUIDService){
        if (_localStorage.isIssetKey('user')){
            this._user.next(_localStorage.read('user'));
            this._clientConnectionService.subscribe(this.user.login.tokens.token).subscribe();
        }
    }

    

    forceRunAuthGuard(): void {
        /*console.log(this._route);
        if (this._route.children.length && this._route.children['0'].snapshot._routeConfig.canActivate) {
            const curr__route = this._route.children[ '0' ];
            const AuthGuard = curr__route.snapshot._routeConfig.canActivate[ '0' ];
            const authGuard = this._injector.get(AuthGuard);
            const _routerStateSnapshot: RouterStateSnapshot = Object.assign({}, curr__route.snapshot, {url: "/"+curr__route.snapshot.url[0]});
            authGuard.canActivate(curr__route.snapshot, _routerStateSnapshot);
        }*/
    }

    // local

    localLogout(): void {
        this.user = null;
        this._clientConnectionService.unsubscribe().subscribe();
        //this.forceRunAuthGuard();
    }

    localLogin(login: Loginned): void {
        const user = new User();
        user.login = login;
        this.user = user;
        this._clientConnectionService.subscribe(login.tokens.token).subscribe();
    }

    localSetLoginTokens(tokens: LoginnedTokens): void {
        const user = this.user;
        user.login.tokens = tokens;
        this.user = user;
        this._clientConnectionService.subscribe(user.login.tokens.token).subscribe();
    }
}