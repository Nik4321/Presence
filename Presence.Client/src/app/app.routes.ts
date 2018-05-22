import { Routes } from '@angular/router';

import { LoginComponent } from './components/user/login/login.component';
import { RegisterComponent } from './components/user/register/register.component';

export const ROUTES: Routes = [
	// Have a default home component which to be served as a landing page
	// { path: '', redirectTo: 'home', pathMatch: 'full' },
	{ path: 'register', component: RegisterComponent },
	{ path: 'login', component: LoginComponent }
];
