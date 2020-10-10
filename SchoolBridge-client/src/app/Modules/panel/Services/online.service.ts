import { Injectable } from '@angular/core';
import { BaseService } from 'src/app/Services/base.service';
import { Service } from 'src/app/Interfaces/Service/service.interface';
import { apiConfig } from 'src/app/Const/api.config';
import { ShortUserModel } from '../Models/short-user.model';
import { UserModel } from '../Models/user.model';
import { Subject, Observable, BehaviorSubject, of } from 'rxjs';
import { debounceTime, tap, bufferWhen, mergeMap, map, filter } from 'rxjs/operators';
import { ServerHub } from 'src/app/Services/server.hub';
import { ClientConnectionService } from 'src/app/Services/client-connection.service';
import { UserService } from 'src/app/Services/user.service';

@Injectable({providedIn: 'root'})
export class OnlineService {
    private _onChangeOnline: BehaviorSubject<Record<string, number>> = new BehaviorSubject<Record<string, number>>({});

    constructor(private _serverHub: ServerHub,
                private _connService: ClientConnectionService,
                private _userService: UserService) {
        this._serverHub.registerOnServerEvent("onlineStatusCheck", (userId: string, onlineStatus:number) => {
            this.changeOnline(userId, onlineStatus);
        });

        this._userService.userObs.subscribe(x => {
            if (!x)
                this._onChangeOnline.next({});
        });
    }

    public changeOnline(userId: string, status: number){
        this._onChangeOnline.value[userId] = status;
        this._onChangeOnline.next(this._onChangeOnline.value);
    }

    public subscribe(model: UserModel): Observable<number>{
        if (model.id == this._userService.user.login.userId) return of(1);
        this.changeOnline(model.id, model.onlineStatus);
        this._connService.send("onlineSubscribe", model.onlineStatusSubscriptionToken).subscribe();

        return this._onChangeOnline.pipe(
            filter(x => Object.keys(x).includes(model.id)),
            map(x => x[model.id])
        );
    }

    public get stats(): Record<string, number>{
      return this._onChangeOnline.value;
    }
}
