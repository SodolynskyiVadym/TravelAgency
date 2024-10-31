import { Injectable } from '@angular/core';
import { environment } from '../../environment/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserEmailRole } from '../../models/userEmailRole.model';
import { UserEmailPassword } from '../../models/userEmailPassword.model';
import { User } from '../../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserApiService {
  private apiUrl = `${environment.server}/auth`;
  
  constructor(private http : HttpClient) { }

  getUsers(token : string) : Observable<User[]> {
    return this.http.get<User[]>(`${this.apiUrl}/getAllUsers`, {
      headers: {
        'Authorization': `Bearer ${token}`
      }
    });
  }

  login(user: UserEmailPassword) : Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, user);
  }

  signup(user: UserEmailPassword) : Observable<any> {
    return this.http.post(`${this.apiUrl}/signup`, user);
  }

  createUser(user : UserEmailRole, token: string) {
    return this.http.post(`${this.apiUrl}/createEditorAdmin`, user, {
      headers: {
        'Authorization': `Bearer ${token}`
      }
    });
  }

  getUserByToken(token: string) : Observable<UserEmailRole> {
    return this.http.get<UserEmailRole>(`${this.apiUrl}/getUserByToken`, {
      headers: {
        'Authorization': `Bearer ${token}`
      }
    });
  }

  deleteUser(id: number, token: string) {
    return this.http.delete(`${this.apiUrl}/deleteUser/${id}`, {
      headers: {
        'Authorization': `Bearer ${token}`
      }
    });
  }


}
