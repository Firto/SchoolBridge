import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './Components/login/login.component';
import { RegisterComponent } from './Components/register/register.component';
import { EmailRegisterComponent } from './Components/email-register/email-register.component';
import { LoginRegisterGuard } from 'src/app/Guard/login-register.guard';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: EmailRegisterComponent },
  { path: 'endregister', component: RegisterComponent },
  { path: '', redirectTo:'login' }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StartRoutingModule { } 