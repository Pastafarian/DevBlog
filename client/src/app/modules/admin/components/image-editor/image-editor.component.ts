import { Component, Input, Output, EventEmitter } from '@angular/core';
import { ImageEditorDialogComponent, ImageDialogOptions } from './../image-editor-dialog/image-editor-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { DialogService } from '@core/services/dialog.service';
import { DialogData } from '@shared/dialog/dialog-component';

export interface ImageConfig {
	imageUrl: string;
}

@Component({
	selector: 'app-image-editor',
	templateUrl: './image-editor.component.html',
	styleUrls: ['./image-editor.component.scss']
})
export class ImageEditorComponent {

	@Input() imageUrl: string | undefined;

	@Input() roundCropper = false;

	@Input() title = '';

	@Input() minWidth = 300;

	@Input() minHeight = 180;

	@Input() aspectRatio: number;

	@Output() imageDeleted = new EventEmitter<boolean>();

	@Output() imageSaved = new EventEmitter<string>();

	@Input() imageProcessing = false;

	constructor(private dialog: MatDialog, private dialogService: DialogService) {
	}

	setImage() {

		const imageDialogOptions: ImageDialogOptions = {
			aspectRatio: this.aspectRatio,
			roundCropper: this.roundCropper,
			minWidth: this.minWidth,
			minHeight: this.minHeight
		};

		this.dialog.open(ImageEditorDialogComponent, {
			width: '800px',
			position: {
				top: '80px',
			},
			data: imageDialogOptions
		}).afterClosed().subscribe((base64EncodedImage: string) => {

			if (base64EncodedImage) {
				this.imageUrl = base64EncodedImage;
				this.imageSaved.emit(base64EncodedImage);
			}
		});
	}

	deleteImage() {

		const data: DialogData = {
			title: 'Confirm Deletion',
			message: 'Are you you want to delete this image?',
		};

		this.dialogService.confirm(data).subscribe(deleteConfirmed => {
			if (deleteConfirmed) {
				this.imageUrl = '';
				this.imageDeleted.emit(true);
			}
		});

		/*
		const confirmOptions: ConfirmDialogOptions = {
			title: 'Confirm Deletion',
			content: 'Are you you want to delete this image?'
		};

		this.dialog.open(ConfirmDialogComponent, {
			width: '800px',
			position: {
				top: '80px',
			},
			data: confirmOptions
		}).afterClosed().subscribe((deleteConfirmed: boolean) => {
			if (deleteConfirmed) {
				this.imageUrl = '';
				this.imageDeleted.emit(true);
			}
		});*/
	}
}
