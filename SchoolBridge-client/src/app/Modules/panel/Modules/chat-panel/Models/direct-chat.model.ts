import { ShortUserModel } from '../../../Models/short-user.model';
import { ChatModel } from './chat.model';

export class DirectChatModel extends ChatModel {
    user: ShortUserModel;
    typing: boolean;
}
