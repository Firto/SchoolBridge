import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable, of, Subject } from "rxjs";
import { GlobalizationInfo } from "../Models/globalization-info.model";
import { MyLocalStorageService } from "src/app/Services/my-local-storage.service";
import { HttpGlobalizationService } from "./http-globalization.service";
import { delayWhen, filter, map, tap, throttle } from "rxjs/operators";
import { IsLoading } from 'src/app/Helpers/is-loading.class';

@Injectable()
export class GlobalizationInfoService {
  public readonly loading: IsLoading = new IsLoading();

  private _info: BehaviorSubject<GlobalizationInfo> = new BehaviorSubject<
    GlobalizationInfo
  >(null);
  private _infoObs: Observable<
    GlobalizationInfo
  > = this._info.asObservable().pipe(
    delayWhen((x) => (this.loading.status ? this.loading.event.pipe(filter(x => !x)) : of())),
    map((x) => this.info)
  );

  public get infoObs(): Observable<GlobalizationInfo> {
    return this._infoObs;
  }

  public get info(): GlobalizationInfo {
    return this._info.value;
  }
  public set info(val: GlobalizationInfo) {
    this._localStorage.write("gbinfo", val);
    this._info.next(val);
  }

  constructor(private _localStorage: MyLocalStorageService) {
    if (this._localStorage.isIssetKey("gbinfo")) {
      this._info.next(this._localStorage.read("gbinfo"));
    }
  }
}
