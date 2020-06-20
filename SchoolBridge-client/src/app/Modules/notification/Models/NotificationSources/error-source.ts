import { MessageSource } from './message-source';

export interface ErrorSource extends MessageSource {
    id: string;
    additionalInfo: any;
}