import { Store } from '@ngxs/store';
import { GetPosts } from '@store/post/post.actions';

export function Load(
	store: Store
): (() => Promise<boolean>) {
	return (): Promise<boolean> => {
		return new Promise<boolean>((resolve: (a: boolean) => void, reject: (reason?: any) => void): void => {
			store.dispatch([new GetPosts(false)]);
			resolve(true);
		});
	};
}
