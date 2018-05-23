import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app.routing.module';
import { HttpClientModule } from '@angular/common/http';

// Components
import { AppComponent } from './app.component';
import { NavbarComponent } from './components/shared/navbar/navbar.component';

// Services
import { HttpService } from './services/http.service';

// Modules
import { UserModule } from './components/user/user.module';

@NgModule({
	declarations: [
		AppComponent,
		NavbarComponent
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
