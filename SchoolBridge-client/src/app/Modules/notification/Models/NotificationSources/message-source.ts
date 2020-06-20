import { INotificationSource } from '../notification-source.interface';

export interface MessageSource extends INotificationSource {
    message: string;
}