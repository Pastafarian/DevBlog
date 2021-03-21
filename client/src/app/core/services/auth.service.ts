import { Injectable } from '@angular/core';
import { AUTH_CONFIG } from './auth0-variables';
import { Router } from '@angular/router';
import * as auth0 from 'auth0-js';
import { timer } from 'rxjs';
import { ApiService } from '@data/services/api.service';
import { Claim } from '@data/schema/claim';

(window as any).global = window;

@Injectable({
	providedIn: 'root',
})
export class AuthService {

	auth0 = new auth0.WebAuth({
		clientID: AUTH_CONFIG.clientID,
		domain: AUTH_CONFIG.domain,
		responseType: 'token id_token',
		redirectUri: window.location.origin + '/callback',
		audience: 'http://stephenadam.api.io',
		scope: 'openid'
	});

	refreshSub: any;

	constructor(private router: Router, private apiService: ApiService) { }

	public isAdmin(): boolean {
		const claims: Claim[] = JSON.parse(localStorage.getItem('claims') as string);

		if (claims && claims.find(x => x.type === 'http://stephenadam.api.io/isAdmin' && x.value === 'true')) {
			return true;
		}
		return false;
	}

	public login(): void {
		this.auth0.authorize();
	}

	public handleAuthentication(): void {
		this.auth0.parseHash((err: any, authResult: any) => {
			if (authResult && authResult.accessToken && authResult.idToken) {
				this.setSession(authResult);
				this.setUserClaims();
				this.router.navigate(['/']);
			} else if (err) {
				this.router.navigate(['/']);
				console.log(err);
				alert(`Error: ${err.error}. Check the console for further details.`);
			}
		});
	}

	public logout(): void {
		// Remove tokens and expiry time from localStorage
		localStorage.removeItem('access_token');
		localStorage.removeItem('id_token');
		localStorage.removeItem('expires_at');
		localStorage.removeItem('claims');
		this.unscheduleRenewal();
		// Go back to the home route
		this.router.navigate(['/']);
	}

	public isAuthenticated(): boolean {
		// Check whether the current time is past the
		// access token's expiry time
		const expiresAt = JSON.parse(localStorage.getItem('expires_at') || '{}');
		return new Date().getTime() < expiresAt;
	}

	public renewToken() {
		this.auth0.checkSession({}, (err: any, result: any) => {
			if (err) {
				console.log(err);
			} else {
				this.setSession(result);
				this.setUserClaims();
			}
		});
	}

	public scheduleRenewal() {
		if (!this.isAuthenticated()) {
			return;
		}

		this.unscheduleRenewal();

		const expiresAt = JSON.parse(window.localStorage.getItem('expires_at') as string);
		const now = Date.now();
		const expiresIn$ = timer(Math.max(1, expiresAt - now));

		// Once the delay time from above is
		// reached, get a new JWT and schedule
		// additional refreshes
		this.refreshSub = expiresIn$.subscribe(() => {
			this.renewToken();
			this.scheduleRenewal();
		});
	}

	public unscheduleRenewal() {
		if (this.refreshSub) {
			this.refreshSub.unsubscribe();
		}
	}

	private setSession(authResult: any): void {
		// Set the time that the access token will expire at
		const expiresAt = JSON.stringify(authResult.expiresIn * 1000 + new Date().getTime());
		localStorage.setItem('access_token', authResult.accessToken);
		localStorage.setItem('id_token', authResult.idToken);
		localStorage.setItem('expires_at', expiresAt);
		this.scheduleRenewal();
	}

	private setUserClaims() {
		this.apiService.getClaims().subscribe(x => {
			localStorage.setItem('claims', JSON.stringify(x.claims));
		});
	}
}
