import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { BreadCrumbsComponent } from './shared/bread-crumbs/bread-crumbs.component';
import { ReactiveFormsModule } from '@angular/forms';
import { CoreModule } from '@core/core.module';
import { SharedModule } from './shared/shared.module';

@NgModule({
	declarations: [
		AppComponent,
		BreadCrumbsComponent,
	],
	imports: [
		CommonModule,
		BrowserModule,
		AppRoutingModule,
		ReactiveFormsModule,
		BrowserAnimationsModule,
		HttpClientModule,
		CoreModule,
		SharedModule
	],
	exports: [
		BreadCrumbsComponent
	],
	bootstrap: [AppComponent]
})
export class AppModule { }
