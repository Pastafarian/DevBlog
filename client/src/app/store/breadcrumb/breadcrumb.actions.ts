import { BreadCrumbItem } from './breadcrumb.type';

export class SetBreadCrumb {
	static readonly type = '[BREADCRUMB] Set';
	constructor(public payload: { breadCrumbs: BreadCrumbItem[]; }) { }
}
