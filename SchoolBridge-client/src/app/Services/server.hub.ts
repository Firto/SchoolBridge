import { Injectable } from '@angular/core';  
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@aspnet/signalr';  
import { environment } from 'src/environments/environment';
import { Observable, BehaviorSubject, from, timer, of, Subject } from 'rxjs';
import { tap, mergeMap, catchError, finalize } from 'rxjs/operators';
import { runInThisContext } from 'vm';
  
@Injectable()
export class ServerHub {    
  private _connectionState: BehaviorSubject<Boolean> = new BehaviorSubject<Boolean>(false); 

  private __onHubConnecting: boolean = false;
  private _onHubConnecting: Subject<void> = new Subject<void>();
  private _registeredOnServerEvents: Record<string, (...args: any[]) => void> = {};

  public onConnectionChanged:Observable<Boolean> = this._connectionState.asObservable();  
  
  public hubConnection: HubConnection = new HubConnectionBuilder()  
                                            .withUrl(environment.apiUrl + "notify")  
                                            .build();  
  
  public registerOnServerEvent(methodName: string, newMethod: (...args: any[]) => void): void {
    this.hubConnection.on(methodName, newMethod);
    this._registeredOnServerEvents[methodName] = newMethod;
  }

  private registerOnServerEvents(){
    Object.keys(this._registeredOnServerEvents).forEach(x => this.hubConnection.on(x, this._registeredOnServerEvents[x]));
  }

  public send(methodName: string, ...args: any[]): Observable<void> {
    if (this.hubConnection.state == HubConnectionState.Disconnected)
      return this.startConnection().pipe(
        mergeMap(() => from(this.hubConnection.invoke(methodName, ...args)))
      );
    else return from(this.hubConnection.invoke(methodName, ...args)).pipe(
      catchError((err: any, obs: Observable<void>) => 
        this.reconnect(10000).pipe(
          mergeMap(() => this.send(methodName, ...args))
        )
      )
    );
  }

  public startConnection(): Observable<void> {  
    if (this.__onHubConnecting)
      return this._onHubConnecting
    else {
      this.__onHubConnecting = true;
      return from(this.hubConnection.start()).pipe(
        tap(() => {
            console.log('Hub connection started');  
            this.registerOnServerEvents();
            this._connectionState.next(true);
            this._onHubConnecting.next();
        }),
        catchError((err: any, obs: Observable<void>) => this.reconnect(10000)),
        finalize(() => this.__onHubConnecting = false)
      );
      }
  }
  
  private reconnect(msec: number): Observable<void> {
    this._connectionState.next(false); 
    console.log('Error while establishing connection, retrying...');  
    return timer(msec).pipe(mergeMap(() => this.startConnection()));
  }


   
}    