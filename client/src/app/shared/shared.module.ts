import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from './material.module';
import { RouterModule } from '@angular/router';
import { BlogListingItemComponent } from './components/blog-listing-item/blog-listing-item.component';
import { DragAndDropDirective } from './directives/drag-and-drop.directive';
import { ConfirmDialogComponent } from './components/confirm-dialog/confirm-dialog.component';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    MaterialModule
  ],
  declarations: [
    BlogListingItemComponent,
    DragAndDropDirective,
    ConfirmDialogComponent
  ],
  exports: [
    MaterialModule,
    BlogListingItemComponent,
    DragAndDropDirective,
    ConfirmDialogComponent
  ]
})
export class SharedModule { }
