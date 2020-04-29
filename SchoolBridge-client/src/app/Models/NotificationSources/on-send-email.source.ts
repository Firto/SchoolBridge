import { INotificationSource } from '../../Modules/db-notification/Sources/notification-source.interface';

export interface OnSendEmailSource extends INotificationSource {
    email: string;
    ok: boolean;
}