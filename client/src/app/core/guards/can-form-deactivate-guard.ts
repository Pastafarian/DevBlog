/*import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { Observable } from 'rxjs';
import { DialogService } from '@core/services/dialog.service';
import { FormGroup } from '@angular/forms';

export interface CanFormComponentDeactivate {

	canDeactivate: () => Observable<boolean> | Promise<boolean> | boolean;
	form?: FormGroup;

	childFormComponent?: any;
}

@Injectable({
	providedIn: 'root'
})
export class CanFormDeactivateGuard implements CanDeactivate<CanFormComponentDeactivate> {
	constructor(private dialogService: DialogService) { }


	canDeactivate(component: CanFormComponentDeactivate) {

		const form =
			(component.hasOwnProperty('form') && component.form) ||
			(component.hasOwnProperty('childFormComponent') &&
				component.childFormComponent &&
				component.childFormComponent.form);

		if (form) {
			return (
				form.pristine ||
				this.dialogService.confirm({
					message: 'Are you sure you want to discard the current changes?',
					okText: 'Discard changes',
					cancelText: 'No, stay here',
					okColour: 'primary'
				})

			);
		}
		return component.canDeactivate ? component.canDeactivate() : true;
	}
}*/
