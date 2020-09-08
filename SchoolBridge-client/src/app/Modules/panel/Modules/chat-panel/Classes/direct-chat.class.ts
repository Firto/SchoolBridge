import { UsersService } from '../../../Services/users.service';
import { Observable } from 'rxjs';
import { User } from '../../../Clases/user.class';
import { Chat } from './chat.class';
import { DirectChatModel } from '../Models/direct-chat.model';
import { MessageModel } from '../Models/message.model';
import { Message } from './message.class';

export class DirectChat extends Chat {
    private readonly _user: Observable<User>;

    public get user(): Observable<User>{
        return this._user;
    }

    constructor(protected _model: DirectChatModel,
                _usersService: UsersService){
        super(_model, _usersService);
        this._user = this._usersService.get(_model.user);
    }

    public addNewMessage(message: MessageModel){
        this._messages.value.push(new Message(message, this._usersService));
        this._messages.next(this._messages.value);
        console.log("mm");
        if (this._model.user.id == message.sender.id) 
            this._read.next(false);
        else this._read.next(true);
    }
}