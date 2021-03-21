import { Component } from '@angular/core';
import { BreadCrumbState } from '@store/breadcrumb/breadcrumb.state';
import { Observable } from 'rxjs';
import { BreadCrumbItem } from '@store/breadcrumb/breadcrumb.type';
import { Select } from '@ngxs/store';

@Component({
	selector: 'app-bread-crumbs',
	templateUrl: './bread-crumbs.component.html',
	styleUrls: ['./bread-crumbs.component.scss']
})
export class BreadCrumbsComponent {

	@Select(BreadCrumbState.selectBreadcrumbItems) breadcrumbs$: Observable<BreadCrumbItem[]>;

	trackBreadcrumb(index: number, breadcrumb: BreadCrumbItem) {
		return breadcrumb.name + breadcrumb.link;
	}
}
