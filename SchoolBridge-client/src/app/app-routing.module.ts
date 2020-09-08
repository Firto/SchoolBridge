import { NgModule } from '@angular/core';
import { Routes, RouterModule, UrlSegment, UrlSegmentGroup, UrlMatchResult } from '@angular/router';
import { LoginRegisterGuard } from './Guard/login-register.guard';
import { StartComponent } from './Modules/start/main/start.component';
import { PanelComponent } from './Modules/panel/main/panel.component';
import { LoginnedGuard } from './Guard/loginned.guard';
//import { EditGlobalizationComponent } from './Modules/edit-globalization/main/edit-globalization.component';
import { DefaultComponent } from './Components/default/default.component';


export const routes: Routes = [
  { path: 'start',
    component: StartComponent, 
    canActivate: [LoginRegisterGuard],
    runGuardsAndResolvers: 'always',
    loadChildren: () => import('./Modules/start/start.module').then(m => m.StartModule),
    
  },
  { path: 'panel',
    component: PanelComponent, 
    canActivate: [LoginnedGuard],
    runGuardsAndResolvers: 'always',
    loadChildren: () => import('./Modules/panel/panel.module').then(m => m.PanelModule),
  },
  /*{ path: 'ed-gb',
    component: EditGlobalizationComponent,
    loadChildren: () => import('./Modules/edit-globalization/edit-globalization.module').then(m => m.EditGlobalizationModule),
  },*/
  { path: '',
    pathMatch: 'full',
    component: DefaultComponent,
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
