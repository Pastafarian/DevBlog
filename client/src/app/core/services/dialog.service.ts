import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Injectable } from '@angular/core';
import { DialogComponent, DialogData } from '@shared/dialog/dialog-component';
import { Observable } from 'rxjs';

@Injectable({
	providedIn: 'root',
})
export class DialogService {
	constructor(private dialog: MatDialog) { }

	/**
	 * Displays confirm dialog. Value returned by Observable is true if user confirms by hitting Ok, otherwise false.
	 * @param data Either a simple string message or for more advance features a ConfirmDialogData.
	 * @returns An Observable that gets notified when dialog is finished closing.
	 */
	confirm(data: string | DialogData): Observable<boolean> {
		const dlg: MatDialogRef<DialogComponent> = this.dialog.open(DialogComponent, {
			disableClose: true,
			data: typeof data === 'string' ? { message: data } : data
		});

		return dlg.afterClosed();
	}
}
