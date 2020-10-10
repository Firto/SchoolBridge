import { ChatModel } from "../Models/chat.model";
import { UsersService } from "../../../Services/users.service";
import { Message } from "./message.class";
import { BehaviorSubject, Observable, of } from "rxjs";
import { filter, map, tap } from "rxjs/operators";
import { MessageModel } from "../Models/message.model";
import { ChatMessageService } from "../Services/chat-message.service";

export class Chat {
  get model(): ChatModel {
    return this._model;
  }
  get id(): string {
    return this._model.id;
  }
  get direct(): boolean {
    return this._model.direct;
  }
  get lastMessageObs(): Observable<Message> {
    return this._lastMessage;
  }
  get lastMessage(): Message {
    if (this._messages.value.length == 0) return null;
    return this._messages.value[this._messages.value.length - 1];
  }
  get read(): boolean {
    return this._read.value;
  }
  get readObs(): Observable<boolean> {
    return this._read;
  }
  get messagesObs(): Observable<Message[]> {
    return this._messages;
  }
  get messages(): Message[] {
    return this._messages.value;
  }

  protected readonly _messages: BehaviorSubject<
    Message[]
  > = new BehaviorSubject<Message[]>([]);
  protected readonly _lastMessage: Observable<Message> = this._messages.pipe(
    filter((x) => x.length > 0),
    map((x) => x[x.length - 1])
  );
  protected readonly _read: BehaviorSubject<boolean> = new BehaviorSubject<
    boolean
  >(this._model.read);
  protected _isEnd: boolean = false;
  protected _isStart: boolean = false;

  constructor(
    protected _model: ChatModel,
    protected _usersService: UsersService,
    protected _chatMessageService: ChatMessageService
  ) {
    if (_model.lastMessage) {
      this._messages.value.push(
        new Message(_model.lastMessage, this._usersService)
      );
      this._messages.next(this._messages.value);
    }
  }

  public addNewMessage(message: MessageModel) {
    this._messages.value.push(new Message(message, this._usersService));
    this._messages.next(this._messages.value);
    this._read.next(true);
  }

  public addNewMessages(messages: MessageModel[]) {
    this._messages.value.unshift(
      ...messages.map((x) => new Message(x, this._usersService)).reverse()
    );
    this._messages.next(this._messages.value);
  }

  public getMoreMessages(): Observable<Message[]> {
    if (this._isEnd) return of(this._messages.value);

    return this._chatMessageService.getMessages(this).pipe(
      map((x) => {
        if (x.length > 0) {
          this.addNewMessages(x);
          if (x.length != 20) this._isEnd = true;
        } else this._isEnd = true;
        return this._messages.value;
      })
    );
  }

  public setTyping(userIds: string[]){

  }
}
