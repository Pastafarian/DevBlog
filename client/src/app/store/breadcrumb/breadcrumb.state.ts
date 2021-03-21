import { State, Action, StateContext, Selector } from '@ngxs/store';
import { SetBreadCrumb } from './breadcrumb.actions';
import { BreadCrumbItem } from './breadcrumb.type';
import { Injectable } from '@angular/core';

export interface BreadCrumbStateModel {
	breadCrumbs: BreadCrumbItem[];
}

@Injectable()
@State<BreadCrumbStateModel>({
	name: 'breadcrumb',
	defaults: {
		breadCrumbs: []
	}
})
export class BreadCrumbState {

	@Selector()
	static selectBreadcrumbItems(state: BreadCrumbStateModel): BreadCrumbItem[] {

		return [...state.breadCrumbs];
	}

	@Action(SetBreadCrumb)
	set(ctx: StateContext<BreadCrumbStateModel>, { payload }: SetBreadCrumb) {

		ctx.patchState({
			breadCrumbs: payload.breadCrumbs
		});

	}
}
