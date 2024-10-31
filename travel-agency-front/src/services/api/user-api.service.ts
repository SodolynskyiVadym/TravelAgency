import { Injectable } from '@angular/core';
import { environment } from '../../environment/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserEmailRole } from '../../models/userEmailRole.model';

@Injectable({
  providedIn: 'root'
})
export class UserApiService {
  private apiUrl = `${environment.server}/auth`;
  
  constructor(private http : HttpClient) { }

  login(user: any) : Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, user);
  }

  registerUser(user: any) : Observable<string> {
    return this.http.post<string>(`${this.apiUrl}/register`, user);
  }

  createUser(user: any) {
    return this.http.post(`${this.apiUrl}/register`, user);
  }

  getUserByToken(token: string) : Observable<UserEmailRole> {
    return this.http.get<UserEmailRole>(`${this.apiUrl}/getUserByToken`, {
      headers: {
        'Authorization': `Bearer ${token}`
      }
    });
  }
}
