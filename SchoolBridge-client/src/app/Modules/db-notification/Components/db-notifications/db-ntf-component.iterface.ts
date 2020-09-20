import { DataBaseSource } from 'src/app/Modules/notification/Models/NotificationSources/database-source';
import { IDBNSource } from 'src/app/Modules/db-notification/Models/IDBN-source.interface';
import { DbNotification } from '../../Services/db-notification.service';

export interface DbNtfComponent{
    model: DbNotification<IDBNSource>;
}