import { Injectable } from '@angular/core';  
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';  
import { environment } from 'src/environments/environment';
import { Subject } from 'rxjs';
import { Notification } from 'src/app/Models/notification.model'
import { BaseService } from './base.service';
import { apiConfig } from 'src/app/Const/api.config';
import { ToastrService } from 'ngx-toastr';
import { Service } from 'src/app/Interfaces/Service/service.interface';
  
@Injectable()  
export class NotificationService {    
  connectionEstablished:Subject<Boolean> = new Subject<Boolean>();

  reciveNotification:Subject<Notification> = new Subject<Notification>();  
  
  private _hubConnection: HubConnection;  
  private ser: Service;
  constructor(private baseService: BaseService,
              public toastr: ToastrService) { 
    this.ser = apiConfig["notification"];
    
    this.createConnection();  
    this.registerOnServerEvents();
    this.startConnection();
  }  
  
  private createConnection() {  
    this._hubConnection = new HubConnectionBuilder()  
      .withUrl(environment.apiUrl + "notify")  
      .build();  
  }  
  
  private reconnect(msec: number): void {
    this.connectionEstablished.next(false); 
    console.log('Error while establishing connection, retrying...');  
    let t_func = () => this.startConnection();
    setTimeout(t_func, msec);
  }

  private startConnection(): void {  
    this._hubConnection  
      .start()  
      .then(() => {   
        console.log('Hub connection started');  
        this.connectionEstablished.next(true);  
      })  
      .catch(err => { 
         this.reconnect(10000)
      });
    this._hubConnection.onclose(() => {
      this.reconnect(10000) 
    });
  } 

  private registerOnServerEvents(): void {  
    this._hubConnection.on('Notification', (data: any) => {
      console.log(data);
      //this.reciveNotification.next(data);
    }); 
  } 

  public subscribe(token:string): void{
    try{
      console.log("subscribed");
      this._hubConnection.invoke('subscribe', token); // https://codepen.io/sabeelmuttil/pen/yWBrxw
    }catch{

    }
  } 

  public permanentSubscribe(token:string): void{
    try{
      console.log("permanentsubscribe");
      this._hubConnection.invoke('permanentSubscribe', token); // https://codepen.io/sabeelmuttil/pen/yWBrxw
    }catch{

    }
  } 

  public unSubscribe(): void{
    console.log("unsubscribed");
    this._hubConnection.invoke('unsubscribe');
  }
}    