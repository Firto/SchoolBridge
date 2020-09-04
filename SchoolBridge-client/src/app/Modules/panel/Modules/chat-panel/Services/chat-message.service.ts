import { Injectable, Type } from '@angular/core';
import { TextMessageType } from '../Models/Messages/text-message.type';
import { MessageType } from '../Models/Messages/message-type.interface';
import { MessageModel } from '../Models/message.model';

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
    constructor() { 
        
    }

    public attachMessageSource(message: MessageModel){
        message.source = JSON.parse(atob(message.base64Source));
    }

    public convertMessageToString(message: MessageModel){
        return chatMessageConfig[message.type].toString(message.source);
    }
}