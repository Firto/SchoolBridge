import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminPanelComponent } from './Modules/admin-panel/main/admin-panel.component';
import { SelectPanelComponent } from './Components/select-panel/select-panel.component';
import { SettingsComponent } from './Components/settings/settings.component';
import { userPermissionGuardFor } from './Modules/user-permission/user-permission.guard';

const routes: Routes = [
  { path: 'admin',
    component: AdminPanelComponent, 
    canActivate: [userPermissionGuardFor('GetAdminPanel')],
    loadChildren: () => import('./Modules/admin-panel/admin-panel.module').then(m => m.AdminPanelModule),
  },
  { path: 'settings',
    component: SettingsComponent
  },
  { path: '',
    pathMatch: 'full',
    component: SelectPanelComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PanelRoutingModule { }
