import { NgModule } from '@angular/core';
import { Routes, RouterModule, UrlSegment, UrlSegmentGroup, UrlMatchResult } from '@angular/router';
import { LoginRegisterGuard } from './Guard/login-register.guard';
import { StartComponent } from './Modules/start/main/start.component';
import { PanelComponent } from './Modules/panel/main/panel.component';
import { LoginnedGuard } from './Guard/loginned.guard';
import { Route } from '@angular/compiler/src/core';
import { appInjector } from 'src/main';
import { UserService } from './Services/user.service';
import { DefaultComponent } from './Components/default/default.component';


const routes: Routes = [
  { path: 'start',
    component: StartComponent, 
    canActivate: [LoginRegisterGuard],
    loadChildren: () => import('./Modules/start/start.module').then(m => m.StartModule),
    
  },
  { path: 'panel',
    component: PanelComponent, 
    canActivate: [LoginnedGuard],
    loadChildren: () => import('./Modules/panel/panel.module').then(m => m.PanelModule),
  },
  { path: '',
    pathMatch: 'full',
    component: DefaultComponent
  }
  /*{ path: 'login', component: LoginComponent, canActivate: [LoginRegisterGuard] },
  { path: 'admin-panel', 
    component: AdminPanelComponent,
    loadChildren: () => import('./Modules/admin-panel/admin-panel.module').then(m => m.AdminPanelModule) 
  },
  { path: 'register', component: EmailRegisterComponent, canActivate: [LoginRegisterGuard] },
  { path: 'endregister', component: RegisterComponent, canActivate: [LoginRegisterGuard] },*/
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
