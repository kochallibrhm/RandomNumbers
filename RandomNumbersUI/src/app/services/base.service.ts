import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({
    providedIn: 'root'
})
export class BaseService {
    protected apiUrl: string;
    protected http: HttpClient;

    constructor(http: HttpClient) {
        this.http = http;
        this.apiUrl = environment.apiUrl;
    }
}
