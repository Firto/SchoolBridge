import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SetSchoolComponent } from './Components/set-school/set-school.component';

const routes: Routes = [
  { path: 'set-school',
    component: SetSchoolComponent,
  },
  {path: '', pathMatch: "full", redirectTo: "set-school"}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DirectorPanelRoutingModule { }
