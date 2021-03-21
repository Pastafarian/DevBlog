import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';

// TODO: Re-write

export interface DialogData {
	message: string;
	title?: string;

	/**
	 * Hides the cancel button.
	 */
	hideCancel?: boolean;

	/**
	 * Text to display on the Ok Button. Default is 'Ok'.
	 */
	okText?: string;

	/**
	 * Material colour for the ok button. Default is Warn.
	 */
	okColour?: string;

	/**
	 * Text to display on the Cancel buttong. Default is 'Cancel'.
	 */
	cancelText?: string;
	/**
	 * Optional handler for OK click. Dialog will display processing state until observable completes.
	 * If value from Obserable it true the window will then close, it will stay open if false.
	 */
	okHandler?: () => Observable<boolean>;
}

@Component({
	selector: 'app-dialog',
	templateUrl: './dialog.component.html',
	styleUrls: ['./dialog.component.scss']
})
export class DialogComponent {
	processing?: boolean;

	constructor(public dialogRef: MatDialogRef<DialogComponent>, @Inject(MAT_DIALOG_DATA) public data: DialogData) { }

	ok() {
		if (!this.data.okHandler) {
			this.dialogRef.close(true);
			return;
		}
		this.processing = true;
		this.data
			.okHandler()
			.pipe(finalize(() => (this.processing = false)))
			.subscribe(result => result && this.dialogRef.close(true));
	}
}
