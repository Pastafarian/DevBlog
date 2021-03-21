import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BlogArticleComponent } from './page/blog-article/blog-article.component';

const routes: Routes = [
  {
    path: '',
    component: BlogArticleComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BlogArticleRoutingModule { }
