import { Observable } from 'rxjs';
import { OnlineService } from '../Services/online.service';
import { UserModel } from '../Models/user.model';
import { environment } from 'src/environments/environment';

export class User{
    private _onlineStatus: Observable<number> = null;

    get model(): UserModel {
        return this._model;
    }
    get id(): string{
        return this._model.id;
    }
    get date(): string{
        return this._model.id;
    }
    get login(): string{
        return this._model.login;
    }
    get photo(): string{
        return environment.apiUrl + "image/" + this._model.photo;
    }
    get role(): string{
        return this._model.role;
    }
    get onlineStatus(): Observable<number>{
        return this._onlineStatus;
    }

    constructor(private readonly _model: UserModel,
                _onlineService: OnlineService){
        this._onlineStatus = _onlineService.subscribe(_model);
    }
}