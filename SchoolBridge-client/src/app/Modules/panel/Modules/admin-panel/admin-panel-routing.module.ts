import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { GlobalizationComponent } from './Components/globalization/globalization.component';

const routes: Routes = [
  { path: 'globalization',
    component: GlobalizationComponent,
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminPanelRoutingModule { }
