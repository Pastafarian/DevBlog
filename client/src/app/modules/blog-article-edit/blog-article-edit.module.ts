import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BlogArticleEditRoutingModule } from './blog-article-edit-routing.module';
import { BlogArticleEditComponent } from './page/blog-article-edit/blog-article-edit.component';
import { ReactiveFormsModule } from '@angular/forms';
import { ImageCropperModule } from 'ngx-image-cropper';
import { SharedModule } from '@shared/shared.module';
import { AdminModule } from '../admin/admin.module';
@NgModule({
  declarations: [BlogArticleEditComponent],
  imports: [
    CommonModule,
    SharedModule,
    AdminModule,
    BlogArticleEditRoutingModule,
    ReactiveFormsModule,
    ImageCropperModule,
  ]
})
export class BlogArticleEditModule { }
