import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminPanelComponent } from './Modules/admin-panel/main/admin-panel.component';
import { SelectPanelComponent } from './Components/select-panel/select-panel.component';
import { SettingsComponent } from './Components/settings/settings.component';

import { ChatPanelComponent } from './Modules/chat-panel/main/chat-panel.component';
import { UserPermissionGuard } from './Modules/user-permission/user-permission.guard';

const routes: Routes = [
  { path: 'admin',
    component: AdminPanelComponent,
    canActivate: [UserPermissionGuard],
    canLoad: [UserPermissionGuard],
    data: {
        perms: ['GetAdminPanel']
    },
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
    pathMatch: 'prefix',
    component: SelectPanelComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PanelRoutingModule { }
