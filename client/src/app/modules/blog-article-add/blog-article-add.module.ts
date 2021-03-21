import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BlogArticleAddRoutingModule } from './blog-article-add-routing.module';
import { BlogArticleAddComponent } from './page/blog-article-add/blog-article-add.component';
import { SharedModule } from '@shared/shared.module';
import { ReactiveFormsModule } from '@angular/forms';
import { ImageCropperModule } from 'ngx-image-cropper';
import { AdminModule } from '../admin/admin.module';

@NgModule({
  declarations: [BlogArticleAddComponent],
  imports: [
    CommonModule,
    SharedModule,
    AdminModule,
    BlogArticleAddRoutingModule,
    ReactiveFormsModule,
    ImageCropperModule,
  ]
})
export class BlogArticleAddModule { }
