import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environment/environment';
import { Router } from '@angular/router';
import { UserApiService } from '../api/user/user-api.service';
import { UserEmailRole } from '../../models/userEmailRole.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = environment.server;

  constructor(private http: HttpClient, private router: Router, private userApi : UserApiService) { }

  login(username: string, password: string) {
    return this.http.post<{ token: string }>(`${this.apiUrl}/login`, { username, password })
      .subscribe(response => {
        localStorage.setItem('token', response.token);
        this.router.navigate(['/']);
      });
  }

  isAuthenticated(): boolean {
    const token = localStorage.getItem('token');
    return !!token;
  }

  getUserRole(): string | null{
    const token = localStorage.getItem('token');
    if(!token) {
      return null;
    }
    var user = {} as UserEmailRole;
    this.userApi.getUserByToken(token).subscribe(result => {
      user = result;
    });
    return user.role;
  }
}
