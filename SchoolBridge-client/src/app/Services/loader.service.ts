import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { Queue } from '../Collections/queue-collection';

@Injectable()
export class LoaderService {
    private loading: Queue<string> = new Queue<string>();

    private isLoadingBase: BehaviorSubject<string> = new BehaviorSubject<string>(null);
    public isLoading: Observable<string> = this.isLoadingBase.asObservable();

    show(str: string = "Loading...") {
        this.loading.enqueue(str);
        this.isLoadingBase.next(str);
    }

    hide() {
        this.loading.dequeue();
        if (this.loading.isEmpty() && this.isLoadingBase.value != null)
            this.isLoadingBase.next(null);
    }
}