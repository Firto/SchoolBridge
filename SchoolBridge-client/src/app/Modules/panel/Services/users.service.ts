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

@Injectable({providedIn:'root'})
export class UsersService {
    private _ser: Service;
    
    private _toggleMainGetBuffer: Subject<any> = <Subject<any>>(new Subject<any>()).pipe(debounceTime(100));
    private _mainGetThread: Subject<ShortUserModel> = null;

    private _loadedUsers: BehaviorSubject<Record<string, User>> = new BehaviorSubject<Record<string, User>>({});

    constructor(private _baseService: BaseService,
                private _onlineService: OnlineService) { 
        this._ser = apiConfig["users"];

        this._mainGetThread = <Subject<ShortUserModel>>(new Subject<ShortUserModel>()).pipe(
            tap(x => this._toggleMainGetBuffer.next(null)),
            bufferWhen(() => this._toggleMainGetBuffer),
            mergeMap(x => {
                x = x.filter((r, y) => x.findIndex(m => m.id == r.id) == y);
                const mustLoad = x.filter(r => !Object.keys(this._loadedUsers.value).includes(r.id));
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
        )

        this._mainGetThread.subscribe();
    }

    public getMany(model: ShortUserModel[]): Observable<UserModel[]>{
        return this._baseService.send<UserModel[]>(this._ser, "getMany", {getTokens: model.map(x => x.getToken.token)});
    }

    public localLoadUsers(users: UserModel[]){
        users.forEach(x => 
            this._loadedUsers.value[x.id] = new User(x, this._onlineService)
        );
        this._loadedUsers.next(this._loadedUsers.value);
    }

    public get(model: ShortUserModel): Observable<User>{
        this._mainGetThread.next(model);
        return this._loadedUsers.pipe(
            filter(x => Object.keys(x).includes(model.id)),
            map(x => x[model.id])
        );
    }
    
}