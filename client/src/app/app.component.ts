import { Component } from '@angular/core';
import { routerTransition } from '@core/animations/router-transition';
import { AuthService } from '@core/services/auth.service';
import ImageResize from 'quill-image-resize-module';
import Quill from 'quill';
import { RouterOutlet } from '@angular/router';

Quill.register('modules/imageResize', ImageResize);

@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.scss'],
	animations: [routerTransition]
})
export class AppComponent {

	constructor(public auth: AuthService) {
		this.auth.handleAuthentication();
		this.auth.scheduleRenewal();
	}

	getState(outlet: RouterOutlet) {
		return outlet.activatedRouteData.state;
	}
}
