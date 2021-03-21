import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BlogArticleAddComponent } from './page/blog-article-add/blog-article-add.component';

const routes: Routes = [
  {
    path: '',
    component: BlogArticleAddComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BlogArticleAddRoutingModule { }
