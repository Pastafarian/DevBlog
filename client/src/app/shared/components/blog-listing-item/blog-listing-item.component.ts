import { Component, OnInit, Input, ChangeDetectionStrategy } from '@angular/core';
import { AuthService } from '@core/services/auth.service';
import { Router } from '@angular/router';
import { DialogService } from '@core/services/dialog.service';
import { ApiService } from '@data/services/api.service';
import { switchMap, filter } from 'rxjs/operators';
import { Store } from '@ngxs/store';
import { DeletePost } from '@store/post/post.actions';
import { Post } from '@data/schema/post';

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

  edit(): void {
    this.router.navigate(['/admin', 'blog', 'edit', this.post.slug]);
  }

  delete() {
    this.dialogService.confirm('Please confirm you want to delete this article')
      .pipe(
        filter(confirmDelete => confirmDelete),
        switchMap(() => this.apiService.deleteBlogPost(this.post.id)),
        switchMap(() => this.store.dispatch(new DeletePost(this.post.id)))
      ).subscribe();
  }
}

