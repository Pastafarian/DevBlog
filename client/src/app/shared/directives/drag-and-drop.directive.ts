import { Directive, HostListener, Output, EventEmitter, Input, Renderer2, ElementRef } from '@angular/core';

@Directive({
	selector: '[appDragAndDrop]'
})
export class DragAndDropDirective {
	@Output()
	fileDrop: EventEmitter<File> = new EventEmitter<File>();

	@Output()
	fileClick: EventEmitter<void> = new EventEmitter<void>();

	private dragOverColor = '#C0C0C0';
	private defaultColor = '#EEEEEE';
	// private defaultColor = '#FFFFFF';

	@Input() dragOverClass = '';

	constructor(private renderer: Renderer2, private hostElement: ElementRef) { }

	@HostListener('drop', ['$event']) public onDrop(evt: DragEvent) {
		evt.preventDefault();
		evt.stopPropagation();
		this.clearDragOver();
		const files = evt.dataTransfer?.files;
		if (files) {
			this.fileDrop.emit(evt.dataTransfer?.files[0]);
		}
	}

	@HostListener('click', ['$event']) public onClick(evt: MouseEvent) {
		this.fileClick.emit();
	}

	@HostListener('dragover', ['$event']) onDragOver(evt: DragEvent) {
		evt.preventDefault();
		evt.stopPropagation();
		this.setDragOver();
	}

	@HostListener('dragleave', ['$event']) public onDragLeave(evt: DragEvent) {
		this.clearDragOver();
	}

	private setDragOver() {
		if (this.dragOverClass) this.renderer.addClass(this.hostElement.nativeElement, this.dragOverClass);
		else this.renderer.setStyle(this.hostElement.nativeElement, 'background', this.dragOverColor);
	}

	private clearDragOver() {
		if (this.dragOverClass) this.renderer.removeClass(this.hostElement.nativeElement, this.dragOverClass);
		else this.renderer.setStyle(this.hostElement.nativeElement, 'background', this.defaultColor);
	}
}
