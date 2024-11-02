import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { UserApiService } from '../../../services/api/user-api.service';
import { BrowserStorageService } from '../../../services/browser-storage-service.service';
import { Router, RouterModule } from '@angular/router';
import { User } from '../../../models/user.model';
import { UserEmailRole } from '../../../models/userEmailRole.model';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterModule],
  templateUrl: './admin.component.html',
  styleUrls: [
    './admin.component.css',
    '../../../assets/styles/style-table.css',
    '../../../assets/styles/style-input-search.css'
  ]
})

export class AdminComponent implements OnInit {
  users: User[] = [];
  filteredUsers: User[] = [];
  input: string = '';

  constructor(private userApi: UserApiService, private browserStorage: BrowserStorageService, private router: Router) { }


  ngOnInit(): void {
    let token = this.browserStorage.get('token');
    if (token) {
      let user = {} as UserEmailRole;
      this.userApi.getUserByToken(token).subscribe({
        next: (result) => {
          user = result;
          this.userApi.getUsers(token).subscribe({
            next: (users: User[]) => {
              console.log(user);
              this.users = users.filter(u => u.email != user.email);
              console.log(this.users);
              this.filteredUsers = this.users;
            },
            error: () => {
              this.users = [];
            }
          });
        },
        error: () => {
          this.browserStorage.remove('token');
          this.router.navigate(['/login']);
        }
      });
    } else {
      this.browserStorage.remove('token');
      this.router.navigate(['/login']);
    }

  }

  filterUsers() {
    if (this.input === '') {
      this.filteredUsers = this.users
    } else {
      this.filteredUsers = this.users.filter((user) => {
        return user.email.includes(this.input);
      });
    }
  }

  deleteUser(id: number) {
    let token = this.browserStorage.get('token');
    if (token) {
      this.userApi.deleteUser(id, token).subscribe({
        next: () => {
          this.users = this.users.filter((user) => {
            return user.id !== id;
          });
          this.input = '';
          this.filteredUsers = this.users;
        },
        error: () => {
          alert('Error deleting user');
        }
      });
    } else {
      this.browserStorage.remove('token');
      this.router.navigate(['/login']);
    }

  }
}
