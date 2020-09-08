import { OnDestroy } from '@angular/core';
import { ReplaySubject } from 'rxjs';

export class OnUnsubscribe implements OnDestroy {
    protected _destroy: ReplaySubject<any> = new ReplaySubject<any>(1);
    
    ngOnDestroy(): void {
        this._destroy.next(null);
        this._destroy.complete();
        console.log("UNSUB");
    }
}