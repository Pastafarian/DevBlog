import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BlogArticleEditComponent } from './page/blog-article-edit/blog-article-edit.component';

const routes: Routes = [
  {
    path: '',
    component: BlogArticleEditComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BlogArticleEditRoutingModule { }
