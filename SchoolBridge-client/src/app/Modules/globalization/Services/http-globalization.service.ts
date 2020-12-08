import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable, Subject } from "rxjs";
import { GlobalizationInfo } from "../Models/globalization-info.model";
import { Service } from "src/app/Interfaces/Service/service.interface";
import { BaseService } from "src/app/Services/base.service";
import { apiConfig } from "src/app/Const/api.config";
import { GlobalizationInfoService } from "./globalization-info.service";
import { tap, finalize, take, mergeMap } from "rxjs/operators";

@Injectable()
export class HttpGlobalizationService {
  private _ser: Service;

  constructor(
    private _baseService: BaseService,
    private _infoService: GlobalizationInfoService
  ) {
    this._ser = apiConfig["globalization"];
  }

  public getInfo(lang: string = ""): Observable<GlobalizationInfo> {
    this._infoService.loading.status = true;

    return this._baseService
      .send<GlobalizationInfo>(
        this._ser,
        "info",
        null,
        this.createHeaders(lang)
      )
      .pipe(finalize(() => this._infoService.loading.status = false));
  }

  public updateBaseUpdateId(): Observable<GlobalizationInfo> {
    return this._baseService.send<GlobalizationInfo>(
      this._ser,
      "update-baseupdateid",
      null,
      this.createHeaders()
    );
  }

  public setString(name: string, str: string): Observable<any> {
    return this._baseService.send<any>(
      this._ser,
      "add-or-upd-string",
      { name: name, string: str },
      this.createHeaders()
    );
  }

  public getStrings(strs: string[]): Observable<Record<string, string>> {
    return this._baseService.send<Record<string, string>>(
      this._ser,
      "strings",
      { strings: strs },
      this.createHeaders()
    );
  }

  public addLanguage(abbName: string, fullName: string): Observable<any> {
    return this._baseService.send<any>(this._ser, "add-language", {
      abbName,
      fullName,
    });
  }

  public removeLanguage(abbName: string): Observable<any> {
    return this._baseService.get(this._ser, "remove-language", {
      params: { abbName: abbName },
    });
  }

  private createHeaders(lang: string = "") {
    if (lang) return { headers: { "accept-language": lang } };
    else if (this._infoService.info)
      return {
        headers: {
          "accept-language": this._infoService.info.currentLanguage,
          baseupdateid: this._infoService.info.baseUpdateId,
        },
      };
    else return {};
  }
}
