import { Component, OnInit } from '@angular/core';
import { Post } from '@data/schema/post';
import { Store, Select } from '@ngxs/store';
import { SetBreadCrumb } from '@store/breadcrumb/breadcrumb.actions';
import { AuthService } from '@core/services/auth.service';
import { Observable } from 'rxjs';
import { PostState, PostStateModel } from '@store/post/post.state';
import { map } from 'rxjs/operators';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

interface HomeVm {
  posts: Post[];
  isAdmin: boolean;
}

@UntilDestroy()
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  vm$: Observable<HomeVm>;

  constructor(private store: Store, private authService: AuthService) { }

  ngOnInit() {
    this.store.dispatch(new SetBreadCrumb({ breadCrumbs: [] }));

    this.vm$ = this.store.select<PostStateModel>(PostState).pipe(
      map(x => {
        return {
          posts: x.posts,
          isAdmin: this.authService.isAdmin()
        };
      }),
      untilDestroyed(this)
    );
  }
}
