import { Observable, fromEvent, Observer } from 'rxjs';
import { map, first } from 'rxjs/operators';

/**
 * Reads content of file and emits content in observable one complete.
 * Calls FileReader.readAsDataURL() so returns a base64 encoded string.
 */
export function readFileContent(file: File): Observable<string> {
	const fileReader = new FileReader();
	const contentOb = fromEvent(fileReader, 'load').
		pipe(
			map(f => <string>fileReader.result),
			first());

	fileReader.readAsDataURL(file);
	return contentOb;
}

export function loadImageFromBase64String(base64String: string): Observable<HTMLImageElement> {

	return new Observable((observer: Observer<HTMLImageElement>) => {
		const img = new Image();
		img.src = base64String;
		img.onload = () => {
			observer.next(img);
			observer.complete();
		};
		img.onerror = err => {
			observer.error(err);
		};
	});
}

export function getFileFromHtmlInputEvent(event: Event): File | null {
	const input = event.target as HTMLInputElement;
	return input?.files ? input.files[0] : null;
}

export interface ImageConstraints {
	maxWidth?: number;
	maxHeight?: number;
	minWidth?: number;
	minHeight?: number;
}

export function imageMeetsConstraints(image: HTMLImageElement, imageConstraints: ImageConstraints): boolean {

	const maxHeightError = (imageConstraints.maxHeight || false) && image.height > imageConstraints.maxHeight;
	const minHeightError = (imageConstraints.minHeight || false) && image.height < imageConstraints.minHeight;
	const maxWidthError = (imageConstraints.maxWidth || false) && image.width > imageConstraints.maxWidth;
	const minWidthError = (imageConstraints.minWidth || false) && image.width < imageConstraints.minWidth;

	const result = maxHeightError || minHeightError || maxWidthError || minWidthError;

	return !result;
}

/**
 * Returns file extension in lower case.
 */
export function getFileExtension(fileName: string) {
	const dotIndex = fileName.lastIndexOf('.');
	return dotIndex === -1 ? '' :
		fileName.substring(dotIndex + 1, fileName.length).toLocaleLowerCase();
}

export const PermittedImageFileTypeExtensions = ['jpg', 'png'];
