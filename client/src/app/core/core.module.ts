import { APP_INITIALIZER, NgModule, Optional, SkipSelf } from '@angular/core';
import { CommonModule } from '@angular/common';
import { throwIfAlreadyLoaded } from '@core/guards/module-import.guard';
import { CanFormDeactivateGuard } from '@core/guards/can-form-deactivate-guard';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ErrorInterceptor } from '@core/interceptors/error.interceptor';
import { AuthInterceptor } from '@core/interceptors/auth.interceptor';
import { BreadCrumbState } from '@store/breadcrumb/breadcrumb.state';
import { PostState } from '@store/post/post.state';
import { Load } from '../configuration/load';
import { NgxsModule, Store } from '@ngxs/store';
import { environment } from '@env';

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule,
    NgxsModule.forRoot([BreadCrumbState, PostState], { developmentMode: !environment.production })
  ],
  declarations: [],
  providers: [
    CanFormDeactivateGuard,
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    { provide: APP_INITIALIZER, useFactory: Load, deps: [Store], multi: true },
  ]
})
export class CoreModule {
  constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
    throwIfAlreadyLoaded(parentModule, 'CoreModule');
  }
}
