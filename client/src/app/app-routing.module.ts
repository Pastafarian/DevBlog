import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

export const routes: Routes = [
  {
    path: 'contact',
    loadChildren: async () => (await import('./modules/contact/contact.module')).ContactModule,
    pathMatch: 'full',
    //canDeactivate: [CanFormDeactivateGuard], TODO:
    data: { state: 'contact' }
  },
  {
    path: 'blog/:slug',
    loadChildren: async () => (await import('./modules/blog-article/blog-article.module')).BlogArticleModule,
    pathMatch: 'full'
  },
  {
    path: 'hire',
    loadChildren: async () => (await import('./modules/hire-me/hire-me.module')).HireMeModule,
    pathMatch: 'full',
    data: { state: 'hire' }
  },
  {
    path: 'callback',
    loadChildren: async () => (await import('./modules/callback/callback.module')).CallbackModule,
    pathMatch: 'full',
    data: { state: 'callback' }
  },
  {
    path: 'blog',
    loadChildren: async () => (await import('./modules/blog/blog.module')).BlogModule,
    pathMatch: 'full',
    data: { state: 'blog' }
  },
  {
    path: '',
    loadChildren: async () => (await import('./modules/home/home.module')).HomeModule,
    pathMatch: 'full',
    data: { state: 'home' }
  },
  {
    path: 'admin/blog/add',
    loadChildren: async () => (await import('./modules/blog-article-add/blog-article-add.module')).BlogArticleAddModule,
    data: { state: 'blog-article-add' }
  },
  {
    path: 'admin/blog/edit/:slug',
    loadChildren: async () => (await import('./modules/blog-article-edit/blog-article-edit.module')).BlogArticleEditModule,
    data: { state: 'blog-article-edit' }
  },
  {
    path: '**',
    loadChildren: async () => (await import('./modules/not-found/not-found.module')).NotFoundModule
  },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes)
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
