import { MessageModel } from './message.model';

export class ChatModel {
    id: string;
    read: boolean;
    subscribeToken: string;
    direct: boolean;
    lastMessage: MessageModel;
}