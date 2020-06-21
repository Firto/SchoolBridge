import { INotificationSource } from '../../notification/Models/notification-source.interface';

export interface OnReadNtfSource extends INotificationSource {
    last: string;
    count: number;
} 