import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpHandler, HttpRequest, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '@env';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
	constructor() { }

	intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

		const token = localStorage.getItem('access_token');

		if (token && request.url.startsWith(environment.baseUrl)) {
			const clone = request.clone({ headers: request.headers.set('Authorization', 'Bearer ' + token) });
			return next.handle(clone);
		} else {
			return next.handle(request);
		}
	}
}
