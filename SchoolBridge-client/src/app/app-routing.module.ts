import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './Components/home/home.component';
import { LoginComponent } from './Components/login/login.component';
import { RegisterComponent } from './Components/register/register.component';
import { EmailRegisterComponent } from './Components/email-register/email-register.component';
import { LoginRegisterGuard } from './Helpers/login-register.guard';
import { LoginnedGuard } from './Helpers/loginned.guard';


const routes: Routes = [
  { path: 'home',
    component: HomeComponent 
  },
  { path: 'login', component: LoginComponent, canActivate: [LoginRegisterGuard] },
  { path: 'register', component: EmailRegisterComponent, canActivate: [LoginRegisterGuard] },
  { path: 'endregister', component: RegisterComponent, canActivate: [LoginRegisterGuard] },
  { path: '', redirectTo: 'home', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
