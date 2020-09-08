import { INotificationSource } from '../notification-source.interface';

export interface OnReadNtfSource extends INotificationSource {
    last: string;
    count: number;
} 