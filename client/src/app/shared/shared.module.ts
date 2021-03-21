import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from './material.module';
import { RouterModule } from '@angular/router';
import { BlogListingItemComponent } from './components/blog-listing-item/blog-listing-item.component';
import { DragAndDropDirective } from './directives/drag-and-drop.directive';
import { DialogComponent } from './dialog/dialog-component';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    MaterialModule
  ],
  declarations: [
    BlogListingItemComponent,
    DragAndDropDirective,
    DialogComponent
  ],
  exports: [
    MaterialModule,
    BlogListingItemComponent,
    DragAndDropDirective,
    DialogComponent
  ]
})
export class SharedModule { }
