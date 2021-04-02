import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiService } from '@data/services/api.service';
import { Post } from '@data/schema/post';
import { Store } from '@ngxs/store';
import { SetBreadCrumb } from '@store/breadcrumb/breadcrumb.actions';
import { MatSnackBar } from '@angular/material/snack-bar';
import { GetPosts } from '@store/post/post.actions';
import { switchMap, tap } from 'rxjs/operators';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

@UntilDestroy()
@Component({
  selector: 'app-blog-article-edit',
  templateUrl: './blog-article-edit.component.html',
  styleUrls: ['./blog-article-edit.component.scss']
})
export class BlogArticleEditComponent implements OnInit {
  constructor(private route: ActivatedRoute,
    private apiService: ApiService,
    private store: Store,
    public snackBar: MatSnackBar,
    private router: Router
  ) { }

  post: Post;
  processing: boolean;

  ngOnInit() {

    this.route.paramMap.pipe(
      switchMap(params => this.apiService.getBlogPost(params.get('slug') ?? '')),
      tap(post => {
        this.post = post;

        this.store.dispatch(
          new SetBreadCrumb({
            breadCrumbs: [
              { name: 'Home', link: '/' },
              { name: 'Blog', link: '/blog' },
              { name: this.post.title, link: this.post.slug },
              { name: 'Editing ' + this.post.title, last: true }
            ]
          }));
      }),
      untilDestroyed(this)
    ).subscribe();
  }

  formSaved(post: Post) {
    this.apiService.updateBlogPost(post)
      .subscribe(_ => {
        this.store.dispatch(new GetPosts(true));
        this.router.navigate(['/blog']);
      });
  }
}
