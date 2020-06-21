import { Injectable, Injector } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { apiConfig } from 'src/app/Const/api.config';
import { APIResult } from 'src/app/Models/api.result.model';
import { BaseService } from '../../../Services/base.service';
import { Loginned } from 'src/app/Models/loginned.model';
import { NotificationService } from '../../notification/Services/notification.service';
import { Service } from 'src/app/Interfaces/Service/service.interface';
import { PermanentSubscribe } from '../../notification/Models/permanent-subscribe.model';
import { EndRegister } from '../Models/endregister.model';
import { OnSendEmailSource } from '../../notification/Models/NotificationSources/on-send-email.source';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { UserService } from 'src/app/Services/user.service';
import { LoaderService } from 'src/app/Services/loader.service';

@Injectable({ providedIn: 'root' })
export class RegisterService {
    private ser: Service;

    constructor(private baseService: BaseService,
                private userService: UserService,
                private notificationService: NotificationService,
                private loaderService: LoaderService,
                private toastrService: ToastrService,
                private router:Router) {
        this.ser = apiConfig["register"];

        this.notificationService.reciveNotification.subscribe(x => {
            
            if (x.type == "onSendEmail"){
                this.loaderService.hide();
                if ((<OnSendEmailSource>x.source).ok)
                {
                    this.toastrService.success("Succesful sending email to "+ (<OnSendEmailSource>x.source).email +"!");
                    this.router.navigate(["/login"]);
                }
                else this.toastrService.error("Error while sending email to "+ (<OnSendEmailSource>x.source).email +", try again!");
            }
        })
    }

    start(email: string): Observable<APIResult> {
        return this.baseService.send(this.ser, "start", null, { headers: {'UUID':this.userService.uuid}, params: { email: email}}).pipe(map(res => {
            if (res.ok == true){
                this.notificationService.permanentSubscribe((<PermanentSubscribe>res.result).token);
                this.loaderService.show("Wait sending email...");
            }
            return res;
        }));
    }

    end(model: EndRegister): Observable<APIResult> {
        return this.baseService.send(this.ser, "end", model, {headers: {'UUID':this.userService.uuid}}).pipe(map(res => {
            if (res.ok == true)
                this.userService.localLogin(<Loginned>res.result);
            return res;
        }));
    }
}