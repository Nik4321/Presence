import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { LoginComponent } from './components/user/login/login.component';
import { RegisterComponent } from './components/user/register/register.component';
import { NavbarComponent } from './components/shared/navbar/navbar.component';

const routes: Routes = [
	// Have a default home component which to be served as a landing page
	// { path: '', redirectTo: 'home', pathMatch: 'full' },
	{ path: 'register', component: RegisterComponent },
	{ path: 'login', component: LoginComponent }
];

@NgModule({
	imports: [RouterModule.forRoot(routes)],
	exports: [RouterModule],
	declarations: [
		AppComponent,
		NavbarComponent,
		LoginComponent,
		RegisterComponent
	]
})
export class AppRoutingModule { }
