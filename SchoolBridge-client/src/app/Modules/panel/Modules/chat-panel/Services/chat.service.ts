import { Injectable } from '@angular/core';
import { BaseService } from 'src/app/Services/base.service';
import { Service } from 'src/app/Interfaces/Service/service.interface';
import { apiConfig } from 'src/app/Const/api.config';
import { Subject, Observable, of, BehaviorSubject } from 'rxjs';
import { debounceTime, tap, bufferWhen, mergeMap, map, filter } from 'rxjs/operators';
import { UsersService } from '../../../Services/users.service';
import { ChatModel } from '../Models/chat.model';
import { DirectChatModel } from '../Models/direct-chat.model';
import { ServerHub } from 'src/app/Services/server.hub';
import { ChatEventModel } from '../Models/chat-event.model';
import { ChatMessageService } from './chat-message.service';
import { Chat } from '../Classes/chat.class';
import { DirectChat } from '../Classes/direct-chat.class';
import { MessageModel } from '../Models/message.model';
import { UserService } from 'src/app/Services/user.service';

@Injectable({providedIn: 'root'})
export class ChatService {
    private _ser: Service;
    private _chats: BehaviorSubject<Record<string, Chat>> = new BehaviorSubject<Record<string, Chat>>({});

    public chats: Observable<Record<string, Chat>> = this._chats.pipe(mergeMap(x => {
        if (Object.keys(x).length == 0)
           return this.getChats().pipe(map(x => {
                this.localNewChats(x);
                console.log(this._chats.value);
                return this._chats.value;
            })); 
        else return of(x);
    }));

    constructor(private _baseService: BaseService,
                private _serverHub: ServerHub,
                private _userService: UserService,
                private _usersService: UsersService,
                private _chatMessageService: ChatMessageService) { 
        this._ser = apiConfig["chat"];

        this._serverHub.registerOnServerEvent("chatEvent", (model: ChatEventModel) => {
            switch (model.type){
                case "newMessage":
                    this._chats.value[model.chatId].addNewMessage(<MessageModel>model.source);
                    break;
            }
        });

        this._userService.userObs.subscribe(x => {
            if (!x)
                this._chats.next({});
        });
    }

    public localNewChat(chat: ChatModel): Chat{
        if (chat.direct === true)
            this._chats.value[chat.id] = new DirectChat(<DirectChatModel>chat, this._usersService);
        this.subscribe(chat.subscribeToken);
        return this._chats.value[chat.id];
    }

    public localNewChats(chats: ChatModel[]): Chat[]{
        return chats.map(chat => this.localNewChat(chat));
    }

    public subscribe(token: string){
        this._serverHub.send("chatSubscribe", token).subscribe();
    }

    public getChats(): Observable<ChatModel[]>{
        return this._baseService.send<ChatModel[]>(this._ser, "getChats");
    }

    public readChat(): Observable<any>{
        return this._baseService.send<any>(this._ser, "getChats");
    }
}