import { DataBaseSource } from 'src/app/Models/NotificationSources/database-source';
import { IDBNSource } from 'src/app/Modules/db-notification/Sources/DataBaseSources/IDBN-source.interface';

export interface DbNtfComponent{
    baseSource: DataBaseSource;
    source: IDBNSource;
}