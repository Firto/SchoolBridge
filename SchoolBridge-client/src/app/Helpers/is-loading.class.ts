import { Observable, Subject } from 'rxjs';

export class IsLoading{
  private _status: boolean = false;
  private _event: Subject<boolean> = new Subject<boolean>();

  public event: Observable<boolean> = this._event;

  public get status(): boolean{
    return this._status;
  }

  public set status(val: boolean){
    this._status = val;
    this._event.next(val);
  }

  constructor(){}


}
