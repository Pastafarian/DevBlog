import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ApiService } from '@data/services/api.service';
import { Post } from '@data/schema/post';
import { Store } from '@ngxs/store';
import { SetBreadCrumb } from '@store/breadcrumb/breadcrumb.actions';
import { Observable } from 'rxjs';
import { map, switchMap, tap } from 'rxjs/operators';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

interface Vm {
  post: Post | null;
}

@UntilDestroy()
@Component({
  selector: 'app-blog-article',
  templateUrl: './blog-article.component.html',
  styleUrls: ['./blog-article.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class BlogArticleComponent implements OnInit {
  constructor(private route: ActivatedRoute, private apiService: ApiService, private store: Store) { }

  vm$: Observable<Vm>;

  post: Post | null;

  ngOnInit() {

    this.vm$ = this.route.params.pipe(
      switchMap(params => {
        return this.apiService.getBlogPost(params['slug']);
      }),
      tap(post => {
        this.store.dispatch(
          new SetBreadCrumb({
            breadCrumbs: [
              { name: 'Home', link: '/' },
              { name: 'Blog', link: '/blog' },
              { name: post.title, last: true }
            ]
          })
        );
      }),
      map(post => {
        return {
          post: post
        };
      }),
      untilDestroyed(this)
    );
  }
}
