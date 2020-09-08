import { MessageModel } from '../Models/message.model';
import { UsersService } from '../../../Services/users.service';
import { User } from '../../../Clases/user.class';
import { Observable } from 'rxjs';
import { MessageType } from '../Models/Messages/message-type.interface';
import { chatMessageConfig } from '../Services/chat-message.service';

export class Message{
    get model(): MessageModel {
        return this._model;
    }
    get id(): string{
        return this._model.id;
    }
    get date(): string{
        return this._model.id;
    }
    get type(): string{
        return this._model.type;
    }
    get sender(): Observable<User>{
        return this._sender;
    }
    get source(): MessageType{
        return this._source;
    }

    private readonly _sender: Observable<User>;
    private readonly _source: MessageType;

    constructor(private readonly _model: MessageModel,
                _usersService: UsersService){
        this._sender = _usersService.get(this._model.sender);
        this._source = JSON.parse(atob(_model.base64Source));
    }

    public toString():string{
        return chatMessageConfig[this._model.type].toString(this._source);
    }
}