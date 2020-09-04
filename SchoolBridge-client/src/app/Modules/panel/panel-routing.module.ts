import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminPanelComponent } from './Modules/admin-panel/main/admin-panel.component';
import { SelectPanelComponent } from './Components/select-panel/select-panel.component';
import { SettingsComponent } from './Components/settings/settings.component';
import { userPermissionGuardFor } from './Modules/user-permission/user-permission.guard';
import { ChatPanelComponent } from './Modules/chat-panel/main/chat-panel.component';

const routes: Routes = [
  { path: 'admin',
    component: AdminPanelComponent, 
    canActivate: [userPermissionGuardFor('GetAdminPanel')],
    loadChildren: () => import('./Modules/admin-panel/admin-panel.module').then(m => m.AdminPanelModule),
  },
  { path: 'chat',
    component: ChatPanelComponent, 
    loadChildren: () => import('./Modules/chat-panel/chat-panel.module').then(m => m.ChatPanelModule),
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
