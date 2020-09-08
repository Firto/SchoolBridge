import { ChatModel } from '../Models/chat.model';
import { UsersService } from '../../../Services/users.service';
import { Message } from './message.class';
import { BehaviorSubject, Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { MessageModel } from '../Models/message.model';

export class Chat {
    get model(): ChatModel {
        return this._model;
    }
    get id(): string{
        return this._model.id;
    }
    get direct(): boolean{
        return this._model.direct;
    }
    get lastMessage(): Observable<Message>{
        return this._lastMessage;
    }
    get read(): Observable<boolean>{
        return this._read;
    }

    protected readonly _messages: BehaviorSubject<Message[]> = new BehaviorSubject<Message[]>([]);
    protected readonly _lastMessage: Observable<Message> = this._messages.pipe(filter(x => x.length > 0), map(x => x[x.length-1]));
    protected readonly _read: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(this._model.read);

    constructor(protected _model: ChatModel,
                protected _usersService: UsersService){
        this._messages.value.push(new Message(_model.lastMessage, this._usersService));
        this._messages.next(this._messages.value);
    }

    public addNewMessage(message: MessageModel){
        this._messages.value.push(new Message(message, this._usersService));
        this._messages.next(this._messages.value);
        this._read.next(true);
    }
}