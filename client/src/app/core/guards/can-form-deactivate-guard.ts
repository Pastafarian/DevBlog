import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { Observable } from 'rxjs';
import { DialogService } from '@core/services/dialog.service';
import { FormGroup } from '@angular/forms';

export interface CanFormComponentDeactivate {
	/**
	 * Method that can be implemented by the routed component to use custom logic.
	 */
	canDeactivate: () => Observable<boolean> | Promise<boolean> | boolean;
	form?: FormGroup;
	/**
	 * If the form is in a direct child then reference it in the parent used for the routing via a childFormComponentProperty.
	 */
	childFormComponent?: any;
}

@Injectable({
	providedIn: 'root'
})
export class CanFormDeactivateGuard implements CanDeactivate<CanFormComponentDeactivate> {
	constructor(private dialogService: DialogService) { }

	// TODO: Re-implement
	/**
	 * There are 2 ways of using this:
	 * 1. The component in question implements it's own canDeactivate() method which will be called.
	 * 2. The compenent in question exposes 'form' or 'childComponent.form' properties on which pristine state will be checked and a confirm message automcatically displayed if dirty.
	 * @param component Interface representing a general form component.
	 */
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
}
