import { ComponentFactoryResolver, ComponentFactory, Injectable } from '@angular/core';
import { DbNtfComponent } from '../Components/db-notifications/db-ntf-component.iterface';
import { dbNotificatonsConfig } from '../Configs/db-notifications.config';

@Injectable({providedIn: 'root'})
export class DbNotificationsMapper{
    constructor(private cmpFactoryResolver: ComponentFactoryResolver){}

    public map(type: string): ComponentFactory<DbNtfComponent>{
        return this.cmpFactoryResolver.resolveComponentFactory(dbNotificatonsConfig[type]);
    }
}