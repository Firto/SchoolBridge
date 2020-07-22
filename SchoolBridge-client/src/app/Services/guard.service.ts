import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class GuardService {
    private _state: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(true);
    public state: Observable<boolean> = this._state.asObservable();

    setState(state: boolean) {
        this._state.next(state);
    }

    getState(): boolean {
        return this._state.value;
    }
}