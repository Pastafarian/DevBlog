import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HireMeRoutingModule } from './hire-me-routing.module';
import { HireMeComponent } from './page/hire-me/hire-me.component';

@NgModule({
  declarations: [HireMeComponent],
  imports: [
    CommonModule,
    HireMeRoutingModule
  ]
})
export class HireMeModule { }
