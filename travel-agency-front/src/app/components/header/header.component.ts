import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { UserApiService } from '../../../services/api/user/user-api.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { UserEmailRole } from '../../../models/userEmailRole.model';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterModule, FormsModule, CommonModule],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})

export class HeaderComponent implements OnInit {
  user = {} as UserEmailRole;

  constructor(private userApi: UserApiService) { }

  ngOnInit(): void {
    if (typeof localStorage !== 'undefined') {
      const token = localStorage.getItem('token');
      if (token !== null) {
        this.userApi.getUserByToken(token).subscribe({
          next: (response) => {
            this.user.role = response.role;
            this.user.email = response.email;
          },
          error: (error) => {
            console.log(error);
            console.log("Removing token");
            localStorage.removeItem('token');
          }
        });
      } else{
        this.user.role = '';
        this.user.email = '';
      }
    }
  }

  logout() {
    console.log("Removing token");
    localStorage.removeItem('token');
    this.user.role = '';
    this.user.email = '';
  }
}
