import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HireMeComponent } from './page/hire-me/hire-me.component';

const routes: Routes = [
  {
    path: '',
    component: HireMeComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HireMeRoutingModule { }
