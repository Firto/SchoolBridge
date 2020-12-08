import {
  Subject,
  BehaviorSubject,
  Observable,
  concat,
  of,
  forkJoin,
} from "rxjs";
import { MyLocalStorageService } from "src/app/Services/my-local-storage.service";
import {
  tap,
  bufferWhen,
  mergeMap,
  debounceTime,
  map,
  concatAll,
  bufferCount, throttleTime, throttle, delayWhen, finalize
} from "rxjs/operators";
import { Injectable } from "@angular/core";
import { HttpGlobalizationService } from "./http-globalization.service";
import { GlobalizationInfoService } from "./globalization-info.service";
import { IsLoading } from 'src/app/Helpers/is-loading.class';

class GbString {
  private static readonly regex = /\[([a-z-0-9]+)(\s?\,\s?.+)?\]/;

  public readonly baseObs: Observable<string>;
  public readonly id: string;
  public readonly args: (string | GbString)[] = [];
  public readonly concatedObs: Observable<string>;

  constructor(
    private readonly _gbsService: GlobalizationStringService,
    private readonly _parent: GbString,
    str: string
  ) {
    const match = str.match(GbString.regex);
    if (!match || match.length === 0) throw "Error string " + str;
    this.id = match[1];
    this.baseObs = _gbsService.getStringObs(this.id);

    if (match[2])
      match[2].split(/\s?\,\s?/).forEach((r) => {
        if (!r || r.length === 0) return;
        if (!r.match(GbString.regex)) this.args.push(r);
        else this.args.push(new GbString(_gbsService, this, r));
      });
    if (!_parent)
      this.concatedObs = this.baseObs.pipe(
        map((x) => this.createRootString(x))
      );
    //else this.baseObs.subscribe(x => this.update());
  }

  public createString(strs: string[]): string {
    //console.log(strs);
    let som = strs[0];
    strs.forEach((x, i) => {
      if (i === 0) return;
      som = som.replace("$arg$", x);
    });
    return som;
  }

  public createRootString(root: string): string {
    return this.createString([
      root,
      ...this.args.map((x) => (typeof x === "string" ? x : x.createMyString())),
    ]);
  }

  public createMyString(): string {
    return this.createRootString(
      this._gbsService.getLoadedStringSave(this.id, "-none-")
    );
  }

  public update(){
    if (this._parent)
      this._parent.update();
    else this._gbsService.updateString(this.id);
  }
}

@Injectable()
export class GlobalizationStringService {
  private _stringDATA: Record<string, Record<string, string>> = {};
  private get _stringD() {
    if (this._gbInfoService.info){
      if (this._stringDATA[this._gbInfoService.info.currentLanguage])
        return this._stringDATA[this._gbInfoService.info.currentLanguage];
      return this._stringDATA[this._gbInfoService.info.currentLanguage] = {};
    }
    return {};
  }
  private _toggleMainGetBuffer: Subject<any> = <Subject<any>>(
    new Subject<any>().pipe(
      debounceTime(10),
      delayWhen(x => this._gbInfoService.infoObs),
    )
  );
  private _mainGetThread: Subject<string> = null;

  private _initializedStrings: Record<string, BehaviorSubject<string>> = {};

  public readonly loading: IsLoading = new IsLoading();

  constructor(
    private _localStorage: MyLocalStorageService,
    private _httpGbService: HttpGlobalizationService,
    private _gbInfoService: GlobalizationInfoService
  ) {
    if (this._localStorage.isIssetKey("gbdata"))
      this._stringDATA = this._localStorage.read("gbdata");

    this._gbInfoService.infoObs.subscribe((x) => {
      if (x) {
        if (!Object.keys(this._stringDATA).includes(x.currentLanguage))
          this._stringDATA[x.currentLanguage] = {};
      }
    });

    this._mainGetThread = <Subject<string>>new Subject<string>().pipe(
      tap((x) => this._toggleMainGetBuffer.next(null)),
      bufferWhen(() => this._toggleMainGetBuffer),
      mergeMap((x) => {
        x.forEach((x) => {
          if (this._initializedStrings[x])
            this._initializedStrings[x].next("-loading-");
        });
        this.loading.status = true;
        return this._httpGbService.getStrings(x).pipe(
          map((r) => {
            Object.assign(this._stringD, r);
            this._localStorage.write("gbdata", this._stringDATA);
            x.forEach((m) => {
              if (!Object.keys(r).includes(m))
                r[m] = this.getLoadedStringSave(m, "-none-");
            });
            Object.keys(r).forEach((m) => {
              if (
                Object.keys(this._initializedStrings).includes(m) &&
                this._initializedStrings[m]
              )
                this._initializedStrings[m].next(r[m]);
            });
            return "";
          }),
          finalize(() => this.loading.status = false)
        );
      }),

    );

    this._mainGetThread.subscribe();
  }

  public loadAllNoLoadedString() {
    Object.keys(this._initializedStrings).forEach((r) => {
      if (!Object.keys(this._stringD).includes(r)) this._mainGetThread.next(r);
      else if (this._initializedStrings[r])
        this._initializedStrings[r].next(this._stringD[r]);
    });
  }

  public updateString(r: string){
    this._initializedStrings[r].next(this._stringD[r]);
  }

  public getLoadedStringSave(str: string, noFund: string): string {
    return Object.keys(this._stringD).includes(str)
      ? this._stringD[str]
      : noFund;
  }

  private isInitializedString(str: string) {
    return Object.keys(this._initializedStrings).includes(str);
  }

  private isLoadedString(str: string) {
    return Object.keys(this._stringD).includes(str);
  }

  public getLoadedStringHttp(str: string, noFund: string): string {
    if (Object.keys(this._stringD).includes(str))
      return this._stringD[str];
    else {
      this._mainGetThread.next(str);
      return noFund;
    }
  }

  public initConstStrings(str: string[]) {
    str.forEach((x) => {
      if (!this.isInitializedString(x))
        this._initializedStrings[x] = null;
      this.getLoadedStringHttp(x, "-loading-");
    });
  }

  public getStringsObs(str: string[]): Record<string, Observable<string>> {
    const obss: Record<string, Observable<string>> = {};
    str.forEach((x) => (obss[x] = this.getStringObs(x)));
    return obss;
  }

  public getStringObs(str: string): Observable<string> {
    if (
      !this.isInitializedString(str) ||
      this._initializedStrings[str] == null
    ) {
      this._initializedStrings[str] = new BehaviorSubject<string>(
        this.getLoadedStringHttp(str, "-loading-")
      );
    }
    return this._initializedStrings[str];
  }

  public convertString(str: string): Observable<string> {
    return new GbString(this, null, str).concatedObs;
  }

  public getConstStringName(str: string) {
    const regex = /\[([a-z-0-9]+)(\s?\,\s?.+)?\]/;
    const match = str.match(regex);
    if (!match || match.length === 0) throw "Error string " + str;
    return match[1];
  }

  public setString(name: string, str: string) {
    this._stringD[name] = str;
    this._localStorage.write("gbdata", this._stringDATA);
    if (
      Object.keys(this._initializedStrings).includes(name) &&
      this._initializedStrings[name]
    )
      this._initializedStrings[name].next(str);
  }

  public clearAllData() {
    this._stringDATA = {};
    this._localStorage.write("gbdata", this._stringDATA);
  }
}
