import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BlogArticleRoutingModule } from './blog-article-routing.module';
import { BlogArticleComponent } from './page/blog-article/blog-article.component';
import { SharedModule } from '@shared/shared.module';

@NgModule({
  declarations: [BlogArticleComponent],
  imports: [
    CommonModule,
    BlogArticleRoutingModule,
    SharedModule
  ]
})
export class BlogArticleModule { }
