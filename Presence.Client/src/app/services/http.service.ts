import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { environment as env } from './../../environments/environment';

@Injectable({
	providedIn: 'root'
})
export class HttpService {
	private http: HttpClient;
	private baseUrl: string;

	constructor(
		http: HttpClient
	) {
		this.http = http;
		this.baseUrl = env.apiUrl;
	}

	public get<T>(
		endpoint: string,
		options?: {
			headers?: HttpHeaders | {
				[header: string]: string | string[];
			};
			observe: 'events';
			params?: HttpParams | {
				[param: string]: string | string[];
			};
			reportProgress?: boolean;
			responseType?: 'json';
			withCredentials?: boolean;
		}
	) {
		return this.http.get<T>(this.getUrl(endpoint), options);
	}

	public post<T>(
		endpoint: string,
		body: any | null,
		options?: {
			headers?:
			| HttpHeaders
			| {
				[header: string]: string | string[];
			};
			observe?: 'body';
			params?:
			| HttpParams
			| {
				[param: string]: string | string[];
			};
			reportProgress?: boolean;
			responseType?: 'json';
			withCredentials?: boolean;
		}
	) {
		return this.http.post<T>(this.getUrl(endpoint), body, options);
	}

	public put<T>(
		endpoint: string,
		body: any | null,
		options?: {
			headers?: HttpHeaders | {
				[header: string]: string | string[];
			};
			observe?: 'body';
			params?: HttpParams | {
				[param: string]: string | string[];
			};
			reportProgress?: boolean;
			responseType?: 'json';
			withCredentials?: boolean;
		}
	) {
		return this.http.put<T>(this.getUrl(endpoint), body, options);
	}

	public delete<T>(
		endpoint: string,
		options?: {
			headers?: HttpHeaders | {
				[header: string]: string | string[];
			};
			observe?: 'body';
			params?: HttpParams | {
				[param: string]: string | string[];
			};
			reportProgress?: boolean;
			responseType?: 'json';
			withCredentials?: boolean;
		}
	) {
		return this.http.delete<T>(this.getUrl(endpoint), options)
	}

	private getUrl(url: string): string {
		return `${this.baseUrl}${url}`;
	}
}
