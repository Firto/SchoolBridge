import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EditUsersComponent } from './Components/edit-users/edit-users.component';
import { GlobalizationComponent } from './Components/globalization/globalization.component';

const routes: Routes = [
  { path: 'globalization',
    component: GlobalizationComponent,
  },
  {
    path: 'edit-usrs',
    component: EditUsersComponent
  },
  { path: '',
    component: GlobalizationComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminPanelRoutingModule { }
