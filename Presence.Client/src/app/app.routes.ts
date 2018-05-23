import { Routes } from '@angular/router';

export const ROUTES: Routes = [
	// Have a default home component which to be served as a landing page
	// { path: '', redirectTo: 'home', pathMatch: 'full' },
	{ path: 'user', loadChildren: './components/user/user.module#UserModule' },
];
