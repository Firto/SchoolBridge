import { INotificationSource } from '../Modules/db-notification/Sources/notification-source.interface';

export class Notification {
    public type:string;
    public source: INotificationSource;
}