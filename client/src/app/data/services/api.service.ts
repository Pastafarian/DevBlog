import { Injectable } from '@angular/core';
import { UrlService } from './url.service';
import { HttpClient } from '@angular/common/http';
import { Post } from '@data/schema/post';
import { Claims } from '@data/schema/claims';
import { SubmitContactRequestDto } from '@data/schema/submitContactRequestDto';
import { Observable } from 'rxjs';

@Injectable({
	providedIn: 'root',
})
export class ApiService {
	constructor(private httpClient: HttpClient, private urlService: UrlService) { }

	postContact(data: SubmitContactRequestDto): Observable<any> {
		return this.httpClient.post(this.urlService.postContact(), data);
	}

	getBlogPosts(): Observable<Post[]> {
		return this.httpClient.get<Post[]>(this.urlService.getPosts());
	}

	getBlogPost(slug: string): Observable<Post> {
		return this.httpClient.get<Post>(this.urlService.getPost(slug));
	}

	updateBlogPost(post: Post): Observable<Post> {
		return this.httpClient.put<Post>(this.urlService.updatePost(), post);
	}

	createBlogPost(post: Post): Observable<Post> {
		return this.httpClient.post<Post>(this.urlService.addPost(), post);
	}

	deleteBlogPost(postId: number): Observable<any> {
		return this.httpClient.delete(this.urlService.deletePost(postId));
	}

	getClaims(): Observable<Claims> {
		return this.httpClient.get<Claims>(this.urlService.getClaims());
	}
}
