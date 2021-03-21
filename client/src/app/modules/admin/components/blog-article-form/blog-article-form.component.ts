import { Component, OnInit, Input, EventEmitter, Output, ViewChildren, QueryList } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Post } from '@data/schema/post';
import { FormMode } from '@data/schema/form-mode';
import { QuillEditorComponent, ContentChange } from 'ngx-quill';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Component({
	selector: 'app-blog-article-form',
	templateUrl: './blog-article-form.component.html',
	styleUrls: ['./blog-article-form.component.scss']
})
export class BlogArticleFormComponent implements OnInit {

	@ViewChildren('editor')
	public editor: QueryList<QuillEditorComponent>;

	@Output()
	formSaved: EventEmitter<any> = new EventEmitter<any>();

	@Input()
	formMode: FormMode;

	@Input()
	post: Post;

	processing = false;
	ready = false;
	form: FormGroup;
	preview: string = '';

	constructor(private fb: FormBuilder) {

	}

	onContentChanged($event: ContentChange) {
		this.preview = $event.html as string;
	}

	onEditorCreated($event: any) {

		this.editor.changes.subscribe((x: QueryList<QuillEditorComponent>) => {

			x.first
				.onContentChanged
				.pipe(
					debounceTime(400),
					distinctUntilChanged()
				)
				.subscribe((data: ContentChange) => {
					// tslint:disable-next-line:no-console
					console.log('view child + directly subscription', data);
				});
		});
	}

	ngOnInit(): void {

		this.form = this.fb.group({
			id: [((this.post && this.post.id) || '') && this.post.id],
			title: [((this.post && this.post.title) || '') && this.post.title, [Validators.required, Validators.maxLength(70), Validators.minLength(1)]],
			slug: [((this.post && this.post.slug) || '') && this.post.slug, [Validators.required, Validators.maxLength(70), Validators.minLength(1)]],
			body: [((this.post && this.post.body) || '') && this.post.body],
			publishDate: [((this.post && this.post.publishDate) || '') && this.post.publishDate, [Validators.required]],
			headerImage: []
		});

		// Set form values dynamically
		if (this.post) {
			for (const [key, value] of Object.entries(this.post)) {
				// console.log(key + ':' + value);
			}
		}

		this.preview = ((this.post && this.post.body) || '') && this.post.body;

		if (this.formMode === 'Add' || (this.formMode === 'Edit' && this.post)) {
			this.ready = true;
		}
	}

	imageSaved(base64Image: string) {
		this.form.patchValue({
			headerImage: base64Image
		});
	}

	imageDeleted() {
		this.form.patchValue({
			imageDeleted: true
		});
	}

	save() {
		if (this.form.valid) {
			this.formSaved.emit(this.form.value);
		} else {
			Object.keys(this.form.controls).forEach(key => {
				this.form.controls[key].markAsTouched();
			});
		}
	}
}
