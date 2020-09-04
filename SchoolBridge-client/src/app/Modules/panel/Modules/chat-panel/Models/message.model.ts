import { ShortUserModel } from '../../../Models/short-user.model';
import { MessageType } from './Messages/message-type.interface';

export class MessageModel {
    id: string;
    date: Date;
    type: string;
    base64Source: string;
    source: MessageType;
    sender: ShortUserModel;
} 