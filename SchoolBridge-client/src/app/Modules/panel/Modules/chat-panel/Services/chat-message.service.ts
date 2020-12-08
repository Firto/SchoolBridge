import { Injectable, Type } from '@angular/core';
import { TextMessageType } from '../Models/Messages/text-message.type';
import { MessageType } from '../Models/Messages/message-type.interface';
import { MessageModel } from '../Models/message.model';
import { BaseService } from 'src/app/Services/base.service';
import { Service } from 'src/app/Interfaces/Service/service.interface';
import { apiConfig } from 'src/app/Const/api.config';
import { Chat } from '../Classes/chat.class';
import { Observable } from 'rxjs';

export const chatMessageConfig: Record<string, { type: Type<MessageType>, toString: (message: MessageType) => string}> = {
    text: {
        type: TextMessageType,
        toString(message: TextMessageType): string{
            return message.text;
        }
    }
}

@Injectable({providedIn: 'root'})
export class ChatMessageService {
    private _ser: Service;
    constructor(private _baseService: BaseService) { 
        this._ser = apiConfig["chat"];
    }

    public getMessages(chat: Chat): Observable<MessageModel[]>{
        return this._baseService.send(this._ser, "getMessages", null, {params: {chatId: chat.model.id, last: chat.messages[0].id}});
    }

    public sendMessage(chat: Chat, text: string): Observable<MessageModel>{
        return this._baseService.send(this._ser, "sendMessage", null, {params: {chatId: chat.model.id, text: text}});
    }

    public convertMessageToString(message: MessageModel){
        return chatMessageConfig[message.type].toString(message.source);
    }
}