import { NgModule } from '@angular/core';
import {  HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BasicAuthInterceptor } from './Intercepors/basic-auth.interceptor';
import { LoaderComponent } from './Components/loader/loader.component';
import { LoaderInterceptor } from './Intercepors/loader.interceptor';
import { SyncRequestService } from 'ts-sync-request/dist'
import { ErrorInterceptor } from './Intercepors/user-error.interceptor';
import { ToastrModule, ToastContainerModule } from 'ngx-toastr';
import { UserService } from './Services/user.service';
import { LoginService } from './Services/login.service';
import { LoaderService } from './Services/loader.service';
import { DeviceUUIDService } from './Services/device-uuid.service';
import { CryptService } from './Services/crypt.service';
import { BaseService } from './Services/base.service';
import { GlobalizationModule } from './Modules/globalization/globalization.module';
import { NotificationModule } from './Modules/notification/notification.module';
import { GuardService } from './Services/guard.service';
import { ServerHub } from './Services/server.hub';
import { ClientConnectionService } from './Services/client-connection.service';
import { MyLocalStorageService } from './Services/my-local-storage.service';
import { CommonModule } from '@angular/common';
import { TimeAgoDirectiveModule } from './Modules/TimeAgoPipe/time-ago-directive.module';
import { ServiceWorkerModule } from '@angular/service-worker';
import { environment } from '../environments/environment';
import { MdGlobalization, MdGlobalizationService } from './Modules/globalization/Services/md-globalization.service';
import { DbNotificationModule } from './Modules/db-notification/db-notification.module';
import { StartModule } from './Modules/start/start.module';
import { PanelModule } from './Modules/panel/panel.module';
import { DefaultComponent } from './Components/default/default.component';
import { BrowserModule } from '@angular/platform-browser';

@NgModule({
  declarations: [
    AppComponent, 
    LoaderComponent,
    DefaultComponent
  ],
  imports: [
    BrowserAnimationsModule,
    CommonModule,
    HttpClientModule,
    AppRoutingModule,
    //ToastContainerModule,
    //ToastrModule.forRoot({ preventDuplicates: true, countDuplicates: true, resetTimeoutOnDuplicate: true }),
    //custom 
    TimeAgoDirectiveModule.forRoot(),
    GlobalizationModule.forRoot(),
    NotificationModule.forRoot(),
    ServiceWorkerModule.register('ngsw-worker.js', { enabled: environment.production })
  ],
  providers: [
    MyLocalStorageService,
    BaseService,
    ServerHub,
    ClientConnectionService,
    UserService,
    LoginService,
    LoaderService,
    DeviceUUIDService,
    SyncRequestService, 
    CryptService,
    GuardService, 
    { provide: HTTP_INTERCEPTORS, useClass: BasicAuthInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: LoaderInterceptor, multi: true } 
  ],
  bootstrap: [AppComponent] 
})
export class AppModule { }

