import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTabsModule } from '@angular/material/tabs';
import { MatRadioModule } from '@angular/material/radio';

@NgModule({
	imports: [
		MatTabsModule,
		MatCardModule,
		MatIconModule,
		MatListModule,
		MatSidenavModule,
		MatInputModule,
		MatDatepickerModule,
		MatNativeDateModule,
		MatSnackBarModule,
		MatDialogModule,
		MatProgressSpinnerModule,
		MatButtonModule,
		MatRadioModule
	],
	exports: [
		MatTabsModule,
		MatCardModule,
		MatIconModule,
		MatListModule,
		MatSidenavModule,
		MatInputModule,
		MatDatepickerModule,
		MatNativeDateModule,
		MatSnackBarModule,
		MatDialogModule,
		MatProgressSpinnerModule,
		MatButtonModule,
		MatRadioModule
	]
})
export class MaterialModule { }
