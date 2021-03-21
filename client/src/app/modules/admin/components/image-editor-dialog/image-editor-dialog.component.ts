import { Component, Inject, ElementRef, ViewChild } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ImageCropperComponent, ImageCroppedEvent } from 'ngx-image-cropper';
import {
	readFileContent, PermittedImageFileTypeExtensions, getFileExtension, loadImageFromBase64String,
	imageMeetsConstraints, getFileFromHtmlInputEvent
} from '@shared/utils/file.util';
import { switchMap } from 'rxjs/operators';

export interface ImageDialogOptions {
	roundCropper: boolean;
	aspectRatio: number;
	minWidth: number;
	minHeight: number;
}

@Component({
	selector: 'app-image-editor-dialog',
	templateUrl: './image-editor-dialog.component.html',
	styleUrls: ['./image-editor-dialog.component.scss']
})
export class ImageEditorDialogComponent {
	imageChangedEvent: any = '';
	imageFilesChangedEvent: File;
	imageBase64ChangedEvent: string;
	imageCroppedEvent: ImageCroppedEvent | null;
	showCropper = false;

	constructor(
		public dialogRef: MatDialogRef<ImageEditorDialogComponent>,
		@Inject(MAT_DIALOG_DATA) public imageDialogOptions: ImageDialogOptions,
		public snackBar: MatSnackBar) {
	}

	@ViewChild(ImageCropperComponent, { static: true }) imageCropper: ImageCropperComponent;
	@ViewChild('fileInput', { static: true }) fileInput: ElementRef;

	fileDropped(file: File): void {
		this.updateCropperImage(file);
	}

	fileChangeEvent(event: Event): void {
		this.updateCropperImage(getFileFromHtmlInputEvent(event));
		this.fileInput.nativeElement.value = null;
	}

	updateCropperImage(file: File | null) {

		if (!file || !PermittedImageFileTypeExtensions.includes(getFileExtension(file.name))) {
			this.showSnackBar('File type not allowed. Only jpg and png images are currently accepted.');
			return;
		}

		readFileContent(file).pipe(
			switchMap(base64String => loadImageFromBase64String(base64String)))
			.subscribe(image => {
				if (imageMeetsConstraints(image, { minHeight: this.imageDialogOptions.minHeight, minWidth: this.imageDialogOptions.minWidth })) {
					this.imageBase64ChangedEvent = image.src;
				} else {
					let error = '';
					if (this.imageDialogOptions.minHeight) error = ` Minimum height ${this.imageDialogOptions.minHeight}.`;
					if (this.imageDialogOptions.minWidth) error += ` Minimum width ${this.imageDialogOptions.minWidth}.`;
					this.showSnackBar(`Sorry, the image must meet the following requirements for this slot. ${error}`);
				}
			});
	}

	imageCropped(event: ImageCroppedEvent) {
		this.imageCroppedEvent = event;
	}

	cropperReady() { }

	imageLoaded() {
		this.showCropper = true;
	}

	loadImageFailed() {
	}

	dropZoneClicked() {
		if (!this.imageCroppedEvent) {
			this.fileInput.nativeElement.click();
		}
	}

	save() {
		if (this.imageCroppedEvent) this.dialogRef.close(this.imageCroppedEvent.base64);
	}

	clear() {
		this.imageCroppedEvent = null;
		this.imageChangedEvent = null;
	}

	cancel() {
		this.dialogRef.close();
	}

	private showSnackBar(message: string) {
		this.snackBar.open(message, '', {
			duration: 4000
		});
	}
}
