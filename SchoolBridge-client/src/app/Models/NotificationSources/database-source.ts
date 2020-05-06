import { INotificationSource } from '../notification-source.interface';

export interface DataBaseSource extends INotificationSource {
   id:string;
   type:string;
   date:string;
   base64Sourse:string;
   read:boolean;
}