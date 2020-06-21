import { IDBNSource } from './IDBN-source.interface';

export interface DBNMessageSource extends IDBNSource{
    message: string;
}