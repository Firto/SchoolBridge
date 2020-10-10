import { Injectable, Injector } from '@angular/core';
import { Observable } from 'rxjs';
import { map, takeUntil, takeWhile, tap } from 'rxjs/operators';

import { apiConfig } from 'src/app/Const/api.config';
import { APIResult } from 'src/app/Models/api.result.model';
import { BaseService } from '../../../Services/base.service';
import { Loginned } from 'src/app/Models/loginned.model';
import { NotificationService } from '../../notification/Services/notification.service';
import { Service } from 'src/app/Interfaces/Service/service.interface';
import { PermanentSubscribe } from '../../notification/Models/permanent-subscribe.model';
import { EndRegister } from '../Models/endregister.model';
import { OnSendEmailSource } from '../../notification/Models/NotificationSources/on-send-email.source';
//import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { UserService } from 'src/app/Services/user.service';
import { LoaderService } from 'src/app/Services/loader.service';
import { PermanentConnectionService } from 'src/app/Modules/start/Services/permanent-connection.service';
import { DeviceUUIDService } from 'src/app/Services/device-uuid.service';
import { GlobalizationStringService } from '../../globalization/Services/globalization-string.service';
import { Toaster } from '../../ngx-toast-notifications';

@Injectable()
export class RegisterService {
    private ser: Service;

    constructor(private _baseService: BaseService,
                private _userService: UserService,
                private _permanentConnectionService: PermanentConnectionService,
                private _notificationService: NotificationService,
                private _loaderService: LoaderService,
                private _toastr: Toaster,
                private _uuidService: DeviceUUIDService,
                private _gbsService: GlobalizationStringService) {
        this.ser = apiConfig["register"];

        this._notificationService.reciveNotification.subscribe(x => {

            if (x.type == "onSendEmail"){
                this._loaderService.hide('wait-snd-em');
                if ((<OnSendEmailSource>x.source).ok)
                  this._toastr.open(this._gbsService.convertString(`[tst-suc-send-em, ${(<OnSendEmailSource>x.source).email}]`), {type: 'success'});
                else this._toastr.open(this._gbsService.convertString(`[tst-unsuc-send-em, ${(<OnSendEmailSource>x.source).email}]`), {type: 'danger'});
            }
        })
    }

    start(email: string): Observable<PermanentSubscribe> {
        return this._baseService.send<PermanentSubscribe>(this.ser, "start", null, { headers: {'UUID':this._uuidService.uuid}, params: { email: email}}).pipe(
            tap(res => {
                //this.router.navigateByUrl("/start");
                this._permanentConnectionService.subscribe(res.token).subscribe();
                this._loaderService.showww('wait-snd-em');
            })
        );
    }

    end(model: EndRegister): Observable<Loginned> {
        return this._baseService.send<Loginned>(this.ser, "end", model, {headers: {'UUID':this._uuidService.uuid}}).pipe(
            tap(res => {
                this._userService.localLogin(<Loginned>res);
            })
        );
    }
}
