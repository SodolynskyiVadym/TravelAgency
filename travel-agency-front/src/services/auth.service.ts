import { Router } from '@angular/router';
import { UserEmailRole } from '../models/userEmailRole.model';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { UserApiService } from './api/user-api.service';
import { DOCUMENT, isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  localStorage: any;

  constructor(private router: Router, private userApi: UserApiService) {
  }

  getToken() {
    return this.localStorage.getItem('token');
  }

  logout() {
    this.localStorage?.removeItem('token');
    this.router?.navigate(['/login']);
  }

  isAuthenticated(): boolean {
    const token = this.localStorage?.getItem('token');
    return !!token;
  }

  getUserRole(): string | null {
    const token = this.localStorage.getItem('token');
    if (!token) {
      return null;
    }
    var user = {} as UserEmailRole;
    this.userApi.getUserByToken(token!).subscribe(result => {
      user = result;
    });
    return user.role;
  }
}
