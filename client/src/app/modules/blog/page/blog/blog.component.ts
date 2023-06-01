import { Component, OnInit } from '@angular/core';
import { Post } from '@data/schema/post';
import { SetBreadCrumb } from '@store/breadcrumb/breadcrumb.actions';
import { Store, Select } from '@ngxs/store';
import { AuthService } from '@core/services/auth.service';
import { BehaviorSubject, Observable } from 'rxjs';
import { PostState } from '@store/post/post.state';
import { map, switchMap, tap } from 'rxjs/operators';
import { MatRadioChange } from '@angular/material/radio';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

interface BlogVm {
  posts: Post[];
  isAdmin: boolean;
}

@UntilDestroy()
@Component({
  selector: 'app-blog',
  templateUrl: './blog.component.html',
  styleUrls: ['./blog.component.scss']
})
export class BlogComponent implements OnInit {

  vm$: Observable<BlogVm>;

  filterTerm$ = new BehaviorSubject('All');
  @Select(PostState.selectFilteredPosts) filteredPosts$: Observable<(filter: string) => Post[]>;

  constructor(private authService: AuthService, private store: Store) { }

  ngOnInit() {

    this.vm$ = this.filterTerm$.pipe(
      tap(() =>
        this.store.dispatch(
          new SetBreadCrumb({
            breadCrumbs: [
              { name: 'Home', link: '/' },
              { name: 'Blog', link: '/blog', last: true }
            ]
          })
        )
      ),
      switchMap(filterTerm => this.store.select<(filter: string) => Post[]>(PostState.selectFilteredPosts).pipe(
        map(filteredPosts => filteredPosts(filterTerm))
      )),
      map(posts => {
        return {
          posts: posts,
          isAdmin: this.authService.isAdmin()
        };
      }),
      untilDestroyed(this));
  }

  postFilterChange(event: MatRadioChange) {
    this.filterTerm$.next(event.value);
  }
}
