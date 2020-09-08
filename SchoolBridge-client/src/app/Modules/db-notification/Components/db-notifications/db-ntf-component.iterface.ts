import { DataBaseSource } from 'src/app/Modules/notification/Models/NotificationSources/database-source';
import { IDBNSource } from 'src/app/Modules/db-notification/Models/IDBN-source.interface';

export interface DbNtfComponent{
    baseSource: DataBaseSource;
    source: IDBNSource;
}