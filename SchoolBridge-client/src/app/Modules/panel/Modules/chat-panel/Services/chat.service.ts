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
import { ClientConnectionService } from 'src/app/Services/client-connection.service';

@Injectable({providedIn: 'root'})
export class ChatService {
    private _ser: Service;
    private _chats: BehaviorSubject<Record<string, Chat>> = new BehaviorSubject<Record<string, Chat>>({});

    public chats: Observable<Chat[]> = this._chats.pipe(map(x => Object.values(x).sort((a: Chat, b: Chat) => {
        if (a.lastMessage && !b.lastMessage) 
            return -1;
        if (b.lastMessage && !a.lastMessage) 
            return 1;
        if (!b.lastMessage && !a.lastMessage) 
            return 0;

        if (a.lastMessage.date > b.lastMessage.date) 
            return -1;
        if (a.lastMessage.date < b.lastMessage.date) 
            return 1;
        
        return 0;
    })));

    constructor(private _baseService: BaseService,
                private _serverHub: ServerHub,
                private _connService: ClientConnectionService,
                private _userService: UserService,
                private _usersService: UsersService,
                private _chatMessageService: ChatMessageService) { 
        this._ser = apiConfig["chat"];

        this._serverHub.registerOnServerEvent("chatEvent", (model: ChatEventModel) => {
            console.log(model);
            switch (model.type){
                case "newMessage":
                    this._chats.value[model.chatId].addNewMessage(<MessageModel>model.source);
                    this._chats.next(this._chats.value);
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
            this._chats.value[chat.id] = new DirectChat(<DirectChatModel>chat, this._usersService, this._chatMessageService);
        this.subscribe(chat.subscribeToken);
        return this._chats.value[chat.id];
    }

    public localNewChats(chats: ChatModel[]): Chat[]{
        return chats.map(chat => this.localNewChat(chat));
    }

    public subscribe(token: string){
        this._connService.send("chatSubscribe", token).subscribe();
    }

    public getChats(): Observable<ChatModel[]>{
        return this._baseService.send<ChatModel[]>(this._ser, "getChats").pipe(tap(x => {
            this.localNewChats(x);
            console.log(this._chats.value);
            this._chats.next(this._chats.value);
        }));
    }

    public readChat(): Observable<any>{
        return this._baseService.send<any>(this._ser, "getChats");
    }
}