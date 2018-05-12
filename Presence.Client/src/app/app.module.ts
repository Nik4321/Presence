import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app.routing.module';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app.component';

// Services
import { HttpService } from './services/http.service';

@NgModule({
	declarations: [],
	imports: [
		BrowserModule,
		AppRoutingModule,
		HttpClientModule
	],
	providers: [HttpService],
	bootstrap: [AppComponent]
})
export class AppModule { }
