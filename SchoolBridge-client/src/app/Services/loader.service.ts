import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { Queue } from '../Collections/queue-collection';

@Injectable()
export class LoaderService {
    private loading: Queue<string> = new Queue<string>();
    private _isLoading: BehaviorSubject<string> = new BehaviorSubject<string>(null);

    public isLoading: Observable<string> = this._isLoading.asObservable();

    show(str: string = "Loading...") {
        this.loading.enqueue(str);
        this._isLoading.next(str);
    }

    hide() {
        this.loading.dequeue();
        if (this.loading.isEmpty() && this._isLoading.value != null)
            this._isLoading.next(null);
    }
}