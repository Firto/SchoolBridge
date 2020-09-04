import { Injectable } from '@angular/core';  
import { Observable } from 'rxjs';
import { ServerHub } from 'src/app/Services/server.hub';

@Injectable()
export class ClientConnectionService {    
  constructor(public serverHub: ServerHub) { }  

  public subscribe(token:string): Observable<void>{
    return this.serverHub.send("subscribe", token);
  } 

  public unsubscribe(): Observable<void> {
    return this.serverHub.send("unSubscribe");
  }
}    