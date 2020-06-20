import { INotificationSource } from './notification-source.interface'

export class Notification {
    public type:string;
    public source: INotificationSource;
}