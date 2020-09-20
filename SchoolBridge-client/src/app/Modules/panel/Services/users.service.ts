import { Injectable } from '@angular/core';
import { BaseService } from 'src/app/Services/base.service';
import { Service } from 'src/app/Interfaces/Service/service.interface';
import { apiConfig } from 'src/app/Const/api.config';
import { ShortUserModel } from '../Models/short-user.model';
import { UserModel } from '../Models/user.model';
import { Subject, Observable, of, BehaviorSubject } from 'rxjs';
import { debounceTime, tap, bufferWhen, mergeMap, map, filter } from 'rxjs/operators';
import { OnlineService } from './online.service';
import { User } from '../Clases/user.class';
import { UserService } from 'src/app/Services/user.service';
import { GlobalizationInfoService } from '../../globalization/Services/globalization-info.service';
import { PanelModule } from '../panel.module';

@Injectable({providedIn: 'root'})
export class UsersService {
    private _ser: Service;
    
    private _toggleMainGetBuffer: Subject<any> = <Subject<any>>(new Subject<any>()).pipe(debounceTime(100));
    private _mainGetThread: Subject<ShortUserModel> = null;

    private _loadedUsers: Record<string, BehaviorSubject<User>> = {};

    constructor(private _baseService: BaseService,
                private _userService: UserService,
                private _onlineService: OnlineService) { 
        this._ser = apiConfig["users"];

        this._mainGetThread = <Subject<ShortUserModel>>(new Subject<ShortUserModel>()).pipe(
            tap(x => this._toggleMainGetBuffer.next(null)),
            bufferWhen(() => this._toggleMainGetBuffer),
            mergeMap(x => {
                x = x.filter((r, y) => x.findIndex(m => m.id == r.id) == y);
                const mustLoad = x.filter(r => !Object.keys(this._loadedUsers).includes(r.id) || !this._loadedUsers[r.id].value);
                if (mustLoad.length > 0) 
                    return this.getMany(mustLoad).pipe(
                        map(x => {
                            this.localLoadUsers(x);
                        })
                    )
                else {
                    this.localLoadUsers([]);
                    return of();
                }
            }),
            map((x) => {
                return new ShortUserModel();
            })
        );

        this._mainGetThread.subscribe();

        this._userService.userObs.subscribe(x => {
            if (!x)
                this._loadedUsers = {};
        });
    }

    public getMany(model: ShortUserModel[]): Observable<UserModel[]>{
        return this._baseService.send<UserModel[]>(this._ser, "getMany", {getTokens: model.map(x => x.getToken.token)});
    }

    public localLoadUsers(users: UserModel[]){
        users.forEach(x => {
            this._loadedUsers[x.id].next(new User(x, this._onlineService))
        });
    }

    public get(model: ShortUserModel): Observable<User>{
        this._mainGetThread.next(model);
        if (!Object.keys(this._loadedUsers).includes(model.id))
            this._loadedUsers[model.id] = new BehaviorSubject<User>(null);
        return this._loadedUsers[model.id];
    }
    
}