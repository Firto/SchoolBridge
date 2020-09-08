import { Injectable } from '@angular/core';  
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@aspnet/signalr';  
import { environment } from 'src/environments/environment';
import { Subject, Observable } from 'rxjs';
import { Notification } from 'src/app/Modules/notification/Models/notification.model'
import { ServerHub } from 'src/app/Services/server.hub';
  
@Injectable()  
export class NotificationService {    
  private _reciveNotification:Subject<Notification> = new Subject<Notification>();  

  public reciveNotification:Observable<Notification> = this._reciveNotification.asObservable();  
 
  constructor(public serverHub: ServerHub) { 
    console.log("registered");
    this.serverHub.registerOnServerEvent('Notification', (data: any) => {
      console.log(data);
      this._reciveNotification.next(data);
    });
  }  
}    