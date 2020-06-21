import { Method } from './service.method.interface';

export interface Service {
    url: string;
    methods?: { [name: string]: Method; }
}