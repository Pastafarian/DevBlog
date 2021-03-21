import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

@Injectable({
	providedIn: 'root',
})
export class HttpService {
	constructor(private httpClient: HttpClient) { }

	get<T>(url: string): Observable<T> {
		return this.httpClient.get<T>(url).pipe(catchError((error: HttpErrorResponse) => this.handleError(error)));
	}

	private handleError(resp: HttpErrorResponse) {
		console.log(resp);
		const message = `Error status code ${resp.status} at ${resp.url}`;
		return throwError(message);
	}
}
