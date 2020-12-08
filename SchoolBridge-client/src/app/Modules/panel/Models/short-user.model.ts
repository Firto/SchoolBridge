import { UserGetTokenModel } from './user-get-token.model';
import { UserModel } from './user.model';
import { Observable } from 'rxjs';

export class ShortUserModel {
    id: string;
    getToken: UserGetTokenModel;
    fullUser: Observable<UserModel>;
}