import { Injectable } from '@angular/core';
import { environment } from '@env';

@Injectable({
	providedIn: 'root',
})
export class UrlService {
	constructor() { }

	postContact() {
		return environment.baseUrl + 'contact';
	}

	getPosts() {
		return environment.baseUrl + 'post';
	}

	updatePost() {
		return environment.baseUrl + 'post/';
	}

	addPost() {
		return environment.baseUrl + 'post/';
	}

	deletePost(postId: number) {
		return environment.baseUrl + 'post/' + postId;
	}

	getPost(slug: string) {
		return environment.baseUrl + 'post/' + slug;
	}

	getClaims() {
		return environment.baseUrl + 'auth/claims';
	}
}
