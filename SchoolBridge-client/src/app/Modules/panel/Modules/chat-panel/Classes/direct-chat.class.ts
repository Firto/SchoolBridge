import { UsersService } from "../../../Services/users.service";
import { BehaviorSubject, Observable } from "rxjs";
import { ShortUser, User } from "../../../Clases/user.class";
import { Chat } from "./chat.class";
import { DirectChatModel } from "../Models/direct-chat.model";
import { MessageModel } from "../Models/message.model";
import { Message } from "./message.class";
import { ChatMessageService } from "../Services/chat-message.service";
import { tap } from "rxjs/operators";

export class DirectChat extends Chat {
  private _typing: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(
    false
  );
  public readonly user: ShortUser;

  public get typingObs(): Observable<boolean> {
    return this._typing;
  }
  public get typing(): boolean {
    return this._typing.value;
  }

  constructor(
    protected _model: DirectChatModel,
    protected _usersService: UsersService,
    protected _chatMessageService: ChatMessageService
  ) {
    super(_model, _usersService, _chatMessageService);
    this.user = this._usersService.get(_model.user);
    this._typing.next(this._model.typing);
  }

  public addNewMessage(message: MessageModel) {
    this._messages.value.push(new Message(message, this._usersService));
    this._messages.next(this._messages.value);
    if (this._model.user.id == message.sender.id) this._read.next(false);
    else this._read.next(true);
  }

  public setTyping(userIds: string[]){
    this._typing.next(userIds.includes(this._model.user.id));
  }
}
