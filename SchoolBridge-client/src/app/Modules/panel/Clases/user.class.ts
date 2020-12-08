import { BehaviorSubject, MonoTypeOperatorFunction, Observable } from "rxjs";
import { OnlineService } from "../Services/online.service";
import { UserModel } from "../Models/user.model";
import { environment } from "src/environments/environment";
import { ShortUserModel } from '../Models/short-user.model';
import { UserGetTokenModel } from '../Models/user-get-token.model';

export class User {
  private _onlineStatus$: Observable<number> = null;

  get model(): UserModel {
    return this._model;
  }
  get id(): string {
    return this._model.id;
  }
  get date(): string {
    return this._model.id;
  }
  get login(): string {
    return this._model.login;
  }
  get photo(): string {
    return environment.apiUrl + "image/" + this._model.photo;
  }
  get role(): string {
    return this._model.role;
  }
  get onlineStatusObs(): Observable<number> {
    return this._onlineStatus$;
  }
  get onlineStatus(): number {
    return this._onlineService.stats[this.id];
  }
  get banned(): boolean {
    return this.model.banned;
  }
  set banned(val: boolean) {
    this.model.banned = val;
  }
  constructor(
    private readonly _model: UserModel,
    private readonly _onlineService: OnlineService
  ) {
    this._onlineStatus$ = _onlineService.subscribe(_model)
  }
}

export class ShortUser{
  pipe(arg0: MonoTypeOperatorFunction<unknown>): import("rxjs").SchedulerLike {
    throw new Error('Method not implemented.');
  }
  private readonly _user$: BehaviorSubject<User>;

  get id(): string {
    return this._model.id;
  }
  get token(): UserGetTokenModel{
    return this._model.getToken;
  }
  get model(): ShortUserModel {
    return this._model;
  }
  get userObs(): Observable<User>{
    return this._user$;
  }
  get user(): User {
    return this._user$.value;
  }
  set user(val: User){
    this._user$.next(val);
  }
  constructor(private readonly _model: ShortUserModel){
    this._user$ = new BehaviorSubject<User>(null);
  }
}
