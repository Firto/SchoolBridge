import { INotificationSource } from '../notification-source.interface';

export interface OnSendEmailSource extends INotificationSource {
    email: string;
    ok: boolean;
}