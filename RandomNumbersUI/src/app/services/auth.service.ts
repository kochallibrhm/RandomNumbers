import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BaseService } from './base.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService extends BaseService {
  private token: string | null = null;
  private authApiUrl: string;

  constructor(http: HttpClient) {
    super(http);
    this.authApiUrl = `${this.apiUrl}/auth`;
    this.loadToken();
  }

  private loadToken(): void {
    this.token = localStorage.getItem('auth_token');
  }

  login(username: string, password: string): Observable<string> {
    return this.http.post<string>(`${this.authApiUrl}/login`, { UserName: username, Password: password }, { responseType: 'text' as 'json' });
  }

  register(username: string, password: string): Observable<boolean> {
    return this.http.post<boolean>(`${this.authApiUrl}/register`, { UserName: username, Password: password }, { responseType: 'text' as 'json' });
  }

  setToken(token: string): void {
    this.token = token;
    localStorage.setItem('auth_token', token);
  }

  getToken(): string | null {
    return this.token;
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  logout(): void {
    this.token = null;
    localStorage.removeItem('auth_token');
  }
}
