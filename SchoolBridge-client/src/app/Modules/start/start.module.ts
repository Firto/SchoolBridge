import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StartRoutingModule } from './start-routing.module';
import { StartComponent } from './main/start.component';
import { RegisterService } from './Services/register.service';
import { LoginComponent } from './Components/login/login.component';
import { EmailRegisterComponent } from './Components/email-register/email-register.component';
import { RegisterComponent } from './Components/register/register.component';
import { StartNavbarComponent } from './Components/navbar/start-navbar.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { GlobalizationModule } from '../globalization/globalization.module';
import { PermanentConnectionService } from './Services/permanent-connection.service';
import { MdGlobalization } from '../globalization/Services/md-globalization.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
    declarations: [
        StartComponent,
        StartNavbarComponent,
        LoginComponent,
        EmailRegisterComponent,
        RegisterComponent
    ],
    providers: [
        RegisterService,
        PermanentConnectionService
    ],
    imports: [
        CommonModule,
        StartRoutingModule,
        GlobalizationModule
    ],
    exports: [
        StartComponent
    ]
})
export class StartModule  {} 