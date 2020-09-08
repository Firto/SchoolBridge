import { Injectable } from '@angular/core';
import { BaseService } from 'src/app/Services/base.service';
import { Service } from 'src/app/Interfaces/Service/service.interface';
import { apiConfig } from 'src/app/Const/api.config';
import { ShortUserModel } from '../Models/short-user.model';
import { UserModel } from '../Models/user.model';
import { Subject, Observable, BehaviorSubject } from 'rxjs';
import { debounceTime, tap, bufferWhen, mergeMap, map, filter } from 'rxjs/operators';
import { ServerHub } from 'src/app/Services/server.hub';

@Injectable({providedIn:'root'})
export class OnlineService {
    private _onChangeOnline: BehaviorSubject<Record<string, number>> = new BehaviorSubject<Record<string, number>>({});
    constructor(private _serverHub: ServerHub) { 
        _serverHub.registerOnServerEvent("onlineStatusCheck", (userId: string, onlineStatus:number) => {
            this.changeOnline(userId, onlineStatus);
        });
    }

    public changeOnline(userId: string, status: number){
        this._onChangeOnline.value[userId] = status;
        this._onChangeOnline.next(this._onChangeOnline.value);
    }

    public subscribe(model: UserModel): Observable<number>{
        this._serverHub.send("onlineSubscribe", model.onlineStatusSubscriptionToken);
        this.changeOnline(model.id, model.onlineStatus);

        return this._onChangeOnline.pipe(
            filter(x => Object.keys(x).includes(model.id)),
            map(x => x[model.id])
        );
    } 
}