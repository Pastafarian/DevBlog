import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ImageEditorComponent } from './components/image-editor/image-editor.component';
import { ImageEditorDialogComponent } from './components/image-editor-dialog/image-editor-dialog.component';
import { BlogArticleFormComponent } from './components/blog-article-form/blog-article-form.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ImageCropperModule } from 'ngx-image-cropper';
import { QuillModule } from 'ngx-quill';
import { SharedModule } from '@shared/shared.module';

@NgModule({
  declarations: [
    ImageEditorComponent,
    ImageEditorDialogComponent,
    BlogArticleFormComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    ImageCropperModule,
    SharedModule,
    QuillModule.forRoot({
      modules: {
        syntax: true,
        toolbar: [
          ['bold', 'italic', 'underline', 'strike'],
          ['blockquote', 'code-block'],
          [{ header: 1 }, { header: 2 }],
          [{ list: 'ordered' }, { list: 'bullet' }],
          [{ script: 'sub' }, { script: 'super' }],
          [{ align: [] }],
          ['clean'],
          ['link', 'image'],
        ],
        imageResize: true
      }
    })
  ],
  exports: [
    ImageEditorComponent,
    ImageEditorDialogComponent,
    BlogArticleFormComponent
  ]
})
export class AdminModule { }
