import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { StartComponent } from '../start/main/start.component';
import { LoginRegisterGuard } from 'src/app/Guard/login-register.guard';
import { PanelComponent } from '../panel/main/panel.component';
import { LoginnedGuard } from 'src/app/Guard/loginned.guard';
import { DefaultEditComponent } from './Components/default/default-edit.component';


const routes: Routes = [
  { path: 'start',
    component: StartComponent, 
    canActivate: [LoginRegisterGuard],
    loadChildren: () => import('../start/start.module').then(m => m.StartModule),
    
  },
  { path: 'panel',
    component: PanelComponent, 
    canActivate: [LoginnedGuard],
    loadChildren: () => import('../panel/panel.module').then(m => m.PanelModule),
  },
  { path: '',
    pathMatch: 'full',
    component: DefaultEditComponent,
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
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EditGlobalizationRoutingModule { }
