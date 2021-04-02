import { Component, OnInit, Input, ChangeDetectionStrategy } from '@angular/core';
import { AuthService } from '@core/services/auth.service';
import { Router } from '@angular/router';
import { ApiService } from '@data/services/api.service';
import { switchMap, filter } from 'rxjs/operators';
import { Store } from '@ngxs/store';
import { DeletePost } from '@store/post/post.actions';
import { Post } from '@data/schema/post';
import { DialogService } from '@app/core/services/dialog-service';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

@UntilDestroy()
@Component({
  selector: 'app-blog-listing-item',
  templateUrl: './blog-listing-item.component.html',
  styleUrls: ['./blog-listing-item.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class BlogListingItemComponent implements OnInit {

  @Input() post: Post;

  constructor(private authService: AuthService,
    private router: Router,
    private dialogService: DialogService,
    private apiService: ApiService,
    private store: Store) { }

  ngOnInit() { }

  isAdmin() {
    return this.authService.isAdmin();
  }

  edit(event: MouseEvent): void {
    event.stopPropagation();
    this.router.navigate(['/admin', 'blog', 'edit', this.post.slug]);
  }

  delete(event: MouseEvent) {
    event.stopPropagation();
    this.dialogService.confirm({ title: 'Confirm deletion', message: 'Are you sure?' })
      .pipe(
        filter(confirmDelete => confirmDelete),
        switchMap(() => this.apiService.deleteBlogPost(this.post.id)),
        switchMap(() => this.store.dispatch(new DeletePost(this.post.id))),
        untilDestroyed(this),
      ).subscribe();
  }
}

