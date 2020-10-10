import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './Components/login/login.component';
import { RegisterComponent } from './Components/register/register.component';
import { EmailRegisterComponent } from './Components/email-register/email-register.component';
import { LoginRegisterGuard } from 'src/app/Guard/login-register.guard';
import { HomeComponent } from './Components/home/home.component';
import { ForgotPasswordComponent } from './Components/forgot-password/forgot-password.component';

const routes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: 'login', component: LoginComponent, data: {animation: 'LoginPage'} },
  { path: 'register', component: EmailRegisterComponent, data: {animation: 'RegisterPage'} },
  { path: 'endregister', component: RegisterComponent, data: {animation: 'EndRegisterPage'} },
  { path: 'forgotpassword', component: ForgotPasswordComponent },
  { path: '', redirectTo: 'home', pathMatch: "prefix" }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StartRoutingModule { } 
