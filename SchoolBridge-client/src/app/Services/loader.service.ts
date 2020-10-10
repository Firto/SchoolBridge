import { Injectable } from "@angular/core";
import { Observable, BehaviorSubject } from "rxjs";
import { filter, map, tap } from "rxjs/operators";
import { Queue } from "../Collections/queue-collection";
import { GlobalizationStringService } from '../Modules/globalization/Services/globalization-string.service';

export class Loading {
  private _text: string;
  public readonly time: Date = new Date();
  public readonly text$: Observable<string>;
  public get text(): string {
    return this._text;
  }
  constructor(public readonly id: string, text$: Observable<string>) {
    this.text$ = text$.pipe(tap((x) => (this._text = x)));
  }
}
@Injectable()
export class LoaderService {
  private _isLoading: BehaviorSubject<
    Record<string, Loading>
  > = new BehaviorSubject<Record<string, Loading>>({});

  constructor(private _gbsService: GlobalizationStringService){}

  public isLoading: Observable<Loading> = this._isLoading.pipe(
    map((x) => {
      if (Object.keys(x).length === 0) return null;
      const arr = Object.values(x).sort(
        (a, b) => a.time.getTime() - b.time.getTime()
      );
      return arr[0];
    })
  );

  show(cfg: Loading) {
    this._isLoading.value[cfg.id] = cfg;
    this._isLoading.next(this._isLoading.value);
  }

  showw(id: string, text$: Observable<string>) {
    this.show(new Loading(id, text$));
  }

  showww(id: string) {
    this.showw(id, this._gbsService.getStringObs(id));
  }

  hide(id: string) {
    if (this._isLoading.value[id]){
      delete this._isLoading.value[id];
      this._isLoading.next(this._isLoading.value);
    }
  }
}
