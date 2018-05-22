import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app.routing.module';
import { HttpClientModule } from '@angular/common/http';

// Components
import { AppComponent } from './app.component';
import { LoginComponent } from './components/user/login/login.component';
import { RegisterComponent } from './components/user/register/register.component';
import { NavbarComponent } from './components/shared/navbar/navbar.component';

// Services
import { HttpService } from './services/http.service';

@NgModule({
	declarations: [
		AppComponent,
		NavbarComponent,
		LoginComponent,
		RegisterComponent
	],
	imports: [
		BrowserModule,
		AppRoutingModule,
		HttpClientModule
	],
	providers: [HttpService],
	bootstrap: [AppComponent]
})
export class AppModule { }
