import { IDBNSource } from './DataBaseSources/IDBN-source.interface';

export interface DBNMessageSource extends IDBNSource{
    message: string;
}