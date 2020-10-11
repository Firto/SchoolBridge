import { Injectable, Injector } from "@angular/core";
import { Observable, BehaviorSubject, Subject } from "rxjs";
import { map, takeUntil, take, tap } from "rxjs/operators";

import { apiConfig } from "src/app/Const/api.config";
import { APIResult } from "src/app/Models/api.result.model";
import { BaseService } from "../../../Services/base.service";
import { Loginned } from "src/app/Models/loginned.model";
import { Service } from "src/app/Interfaces/Service/service.interface";

import { UserService } from "src/app/Services/user.service";
import { ProfileModel } from "../Models/profile.model";
import { SubjectModel } from '../Models/subject.model';

@Injectable()
export class SubjectService {
  private _ser: Service;

  constructor(
    private baseService: BaseService,
    private userService: UserService
  ) {
    this._ser = apiConfig["subject"];

  }

  getAll(): Observable<SubjectModel[]> {
    return this.baseService
      .send<SubjectModel[]>(this._ser, "getAll");
  }

  addOrUpdate(dayNumber: number, lessonNumber: number, subjectName: string, comment: string): Observable<SubjectModel> {
    return this.baseService
      .send<SubjectModel>(this._ser, "addOrUpdate", {dayNumber: dayNumber.toString(), lessonNumber: lessonNumber.toString(), subjectName, comment});
  }
}
