import { INotificationSource } from '../../Modules/db-notification/Sources/notification-source.interface';

export interface MessageSource extends INotificationSource {
    message: string;
}