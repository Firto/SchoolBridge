import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { ServerHub } from 'src/app/Services/server.hub';
import { UserService } from './user.service';
import { filter, mergeMap, tap } from 'rxjs/operators';

@Injectable()
export class ClientConnectionService {
  private _onSubscribed: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  private _subscribed: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  constructor(public serverHub: ServerHub,
              private _userService: UserService) {
    this._userService.userObs.subscribe(x => {

      console.log(x);
      if (x){
        this._onSubscribed.next(true)
        this.subscribe(x.login.tokens.token).subscribe(() => this._onSubscribed.next(false));
      }else this.unsubscribe().subscribe();
    });
  }

  public subscribe(token:string): Observable<void>{
    return this.serverHub.send("subscribe", token).pipe(tap(() => this._subscribed.next(true)));
  }

  public unsubscribe(): Observable<void> {
    return this.serverHub.send("unSubscribe").pipe(tap(() => this._subscribed.next(false)));
  }

  public send(methodName: string, ...args: any[]): Observable<void>{
    if (this._onSubscribed.value)
      return this._onSubscribed.pipe(
        filter(x => !x),
        mergeMap(x => this.serverHub.send(methodName, ...args))
      );
    else if (!this._subscribed.value)
    {
      if (!this._userService.user) throw "no user!";
      this._onSubscribed.next(true)
      return this.subscribe(this._userService.user.login.tokens.token)
            .pipe(
              tap(() => this._onSubscribed.next(false)),
              mergeMap(x => this.serverHub.send(methodName, ...args))
            );
    }
    else return this.serverHub.send(methodName, ...args)
  }
}
