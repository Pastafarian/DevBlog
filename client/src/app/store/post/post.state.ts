import { State, Action, StateContext, Selector } from '@ngxs/store';
import { Injectable } from '@angular/core';
import { Post } from '@data/schema/post';
import { SetPosts, GetPosts, UpdatePost, AddPost, DeletePost } from './post.actions';
import { ApiService } from '@data/services/api.service';
import { switchMap } from 'rxjs/operators';
import { isFresh, TrackedState, lastFetchedNow } from '../helpers/cache-policy';
import { patch, append, removeItem, updateItem } from '@ngxs/store/operators';

export interface PostStateModel extends TrackedState {
	posts: Post[];
}

@Injectable()
@State<PostStateModel>({
	name: 'post',
	defaults: {
		posts: [],
		lastFetched: 0
	}
})
export class PostState {

	constructor(private apiService: ApiService) {
	}

	@Selector()
	static selectPosts(state: PostStateModel): Post[] {
		return [...state.posts];
	}

	@Selector()
	static selectFilteredPosts(state: PostStateModel) {

		return (filter: string) => {

			let posts = state.posts;

			switch (filter) {
				case 'Live':
					posts = posts.filter(p => p.isPublished && Date.parse(p.publishDate) < Date.now());
					break;
				case 'Scheduled':
					posts = posts.filter(p => p.publishDate && Date.parse(p.publishDate) > Date.now());
					break;
				case 'Unpublished':
					posts = state.posts.filter(p => !p.isPublished);
					break;
			}

			return [...posts].sort(x => x.publishDate ? Date.parse(x.publishDate) : Number.MAX_SAFE_INTEGER);
		};
	}

	@Action(SetPosts)
	SetPosts(ctx: StateContext<PostStateModel>, action: SetPosts) {
		ctx.patchState({
			posts: action.posts,
			lastFetched: lastFetchedNow()
		});
	}

	@Action(UpdatePost)
	UpdatePost(ctx: StateContext<PostStateModel>, action: UpdatePost) {
		ctx.setState(
			patch({
				posts: updateItem<Post>(x => x?.id === action.post.id, action.post)
			})
		);
	}

	@Action(AddPost)
	AddPost(ctx: StateContext<PostStateModel>, action: AddPost) {
		ctx.setState(
			patch({
				posts: append([action.post])
			})
		);
	}

	@Action(DeletePost)
	DeletePost(ctx: StateContext<PostStateModel>, action: DeletePost) {
		ctx.setState(
			patch({
				posts: removeItem<Post>(x => x?.id === action.postId)
			})
		);
	}

	@Action(GetPosts)
	get(ctx: StateContext<PostStateModel>, action: GetPosts) {

		const posts = ctx.getState().posts;
		if (!action.forceReload && posts && isFresh(ctx.getState())) return;

		return this.apiService.getBlogPosts().pipe(
			switchMap(x => ctx.dispatch(new SetPosts(x)))
		);
	}
}
