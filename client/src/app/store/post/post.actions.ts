import { Post } from '@data/schema/post';

export class GetPosts {
	static readonly type = '[POST] GetPosts';
	constructor(public forceReload: boolean) { }
}

export class SetPosts {
	static readonly type = '[POST] SetPosts';
	constructor(public posts: Post[]) { }
}

export class AddPost {
	static readonly type = '[POST] AddPost';
	constructor(public post: Post) { }
}

export class UpdatePost {
	static readonly type = '[POST] UpdatePost';
	constructor(public post: Post) { }
}

export class DeletePost {
	static readonly type = '[POST] DeletePost';
	constructor(public postId: number) { }
}
