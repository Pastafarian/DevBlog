import { Component, ViewChild } from '@angular/core';
import { Post } from '@data/schema/post'
import { ApiService } from '@data/services/api.service';
import { Router } from '@angular/router';
import { BlogArticleFormComponent } from './../../../admin/components/blog-article-form/blog-article-form.component';
import { Store } from '@ngxs/store';
import { AddPost } from '@store/post/post.actions';

@Component({
  selector: 'app-blog-article-add',
  templateUrl: './blog-article-add.component.html',
  styleUrls: ['./blog-article-add.component.scss']
})
export class BlogArticleAddComponent {

  @ViewChild(BlogArticleFormComponent, { static: true }) childFormComponent: BlogArticleFormComponent; // Used by CanDeactivateGuard.

  constructor(private apiService: ApiService, private router: Router, private store: Store) { }

  formSaved(post: Post) {
    this.apiService.createBlogPost(post)
      .subscribe(x => {
        this.store.dispatch(new AddPost(x));
        this.router.navigate(['/blog']);
      });
  }
}
