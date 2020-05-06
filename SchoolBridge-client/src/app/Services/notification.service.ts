import { Injectable } from '@angular/core';  
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@aspnet/signalr';  
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

  private startConnection(f: () => void = null): void {  
    try{
    this._hubConnection  
      .start()  
      .then(() => {   
        console.log('Hub connection started');  
        this.connectionEstablished.next(true);
        if (f != null) f(); 
      })
      .catch(err => { 
        this.reconnect(10000) 
      });
    }catch{

    }
  } 

  private registerOnServerEvents(): void {  
    this._hubConnection.on('Notification', (data: any) => {
      console.log(data);
      this.reciveNotification.next(data);
    }); 
  } 

  public subscribe(token:string): void{
    let f = () => {
      try{
      console.log("subscribed");
      this._hubConnection.invoke('subscribe', token); // https://codepen.io/sabeelmuttil/pen/yWBrxw
      }catch{

      }
    };
    if (this._hubConnection.state == HubConnectionState.Connected)
      f();
    else this.startConnection(f);
  } 

  public permanentSubscribe(token:string): void{
    let f = () => {
      try{
        console.log("permanentsubscribe");
        this._hubConnection.invoke('permanentSubscribe', token); // https://codepen.io/sabeelmuttil/pen/yWBrxw
      }catch{
  
      }
    };
    if (this._hubConnection.state == HubConnectionState.Connected)
      f();
    else this.startConnection(f);
  } 

  public unsubscribe(): void{
    let f = () => {
      try{
        console.log("unsubscribed");
        this._hubConnection.invoke('unsubscribe').then(() => this._hubConnection.stop());
        
      }catch{
  
      }
    };
    if (this._hubConnection.state == HubConnectionState.Connected)
      f();
    else this.startConnection(f);
    
  }
}    