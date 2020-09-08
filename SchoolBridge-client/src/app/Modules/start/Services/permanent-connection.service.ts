import { Injectable } from '@angular/core';  
import { Observable } from 'rxjs';
import { ServerHub } from 'src/app/Services/server.hub';

@Injectable()
export class PermanentConnectionService {    
  constructor(public serverHub: ServerHub) { }  

  public subscribe(token:string): Observable<void>{
    return this.serverHub.send("permanentSubscribe", token);
  } 

  public unsubscribe(): Observable<void> {
    return this.serverHub.send("unsubscribe");
  }
}    