import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './Components/home/home.component';
import { GlobalizationComponent } from './Components/globalization/globalization.component';

const routes: Routes = [
  { path: 'home',
    component: HomeComponent,
  },
  { path: 'globalization',
    component: GlobalizationComponent,
  },
  {path: '', pathMatch: "full", redirectTo: "home"}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminPanelRoutingModule { }
