import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {  HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { HomeComponent } from './Components/home/home.component';
import { LoginComponent } from './Components/login/login.component';
import { RegisterComponent } from './Components/register/register.component';
import { NavbarComponent } from './Components/navbar/navbar.component';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BasicAuthInterceptor } from './Helpers/basic-auth.interceptor';
import { LoginService } from './Services/login.service';
import { LoaderComponent } from './Components/loader/loader.component';
import { LoaderService } from './Helpers/loader.service';
import { LoaderInterceptor } from './Helpers/loader.interceptor';
import { DeviceUUIDService } from './Helpers/device-uuid.service';
import { SyncRequestService } from 'ts-sync-request/dist'
import { ErrorInterceptor } from './Helpers/user-error.interceptor';
import { CryptService } from './Helpers/crypt.service';
import { NotificationService } from './Services/notification.service';
import { ToastrModule } from 'ngx-toastr';
import { DbNotificationModule } from './Modules/db-notification/db-notification.module';
import { UserService } from './Services/user.service';
import { EmailRegisterComponent } from './Components/email-register/email-register.component';
import { RegisterService } from './Services/register.service';

@NgModule({
  declarations: [
    AppComponent, 
    HomeComponent,
    LoginComponent,
    RegisterComponent,
    EmailRegisterComponent,
    LoaderComponent,
    NavbarComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    FormsModule,
    ToastrModule.forRoot(),
    DbNotificationModule
  ],
  providers: [
    LoginService,
    UserService,
    LoaderService,
    DeviceUUIDService,
    SyncRequestService, 
    CryptService,
    RegisterService,
    NotificationService,
    { provide: HTTP_INTERCEPTORS, useClass: BasicAuthInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: LoaderInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

