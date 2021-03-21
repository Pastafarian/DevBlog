import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable, EMPTY } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
	constructor(public snackBar: MatSnackBar) {}

	intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
		return next.handle(request).pipe(
			catchError(err => {
				const error = err.message || err.statusText;
				console.log('Http error calling api - ' + error);
				this.snackBar.open('Sorry, the site is having problems at the moment - ' + error, '', {
					duration: 3000
				});
				return EMPTY;
			})
		);
	}
}
