import { Router } from '@angular/router';
import { UserEmailRole } from '../models/userEmailRole.model';
import { UserApiService } from './api/user-api.service';
import { BrowserStorageService } from './browser-storage-service.service';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private router: Router, private userApi: UserApiService, private browserStorage : BrowserStorageService) {
  }

  getToken() {
    return this.browserStorage.get('token');
  }

  logout() {
    this.browserStorage.remove('token');
    console.log('Logged out');
    this.router?.navigate(['/']);
  }

  isAuthenticated(): boolean {
    const token = this.browserStorage.get('token');
    return !!token;
  }

  getUserRole(): UserEmailRole | null {
    console.log("Taking token from browser storage in auth.service.getUserRole");
    const token = this.browserStorage.get('token');
    console.log("Token in auth.service.getUserRole", token);
    if (!token) {
      return null;
    }
    var user = {} as UserEmailRole;
    this.userApi.getUserByToken(token).subscribe(result => {
      console.log(result)
      user = result;
    });
    return user;
  }
}

