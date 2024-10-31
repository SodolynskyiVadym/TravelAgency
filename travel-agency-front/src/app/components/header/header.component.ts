import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { UserApiService } from '../../../services/api/user-api.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { UserEmailRole } from '../../../models/userEmailRole.model';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterModule, FormsModule, CommonModule],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})

export class HeaderComponent implements OnInit {
  user = {} as UserEmailRole;
  token: string | null = null;
  isAuthenticated = false;

  constructor(private userApi: UserApiService, private auth : AuthService) { }

  ngOnInit(): void {
    this.token = this.auth.getToken();
    if(this.token){
      this.userApi.getUserByToken(this.token).subscribe({
        next: (response) => {
          this.user.role = response.role;
          this.user.email = response.email;
        },
        error: (error) => {
          console.log(error);
          this.auth.logout();
        }
      });
    }
  }

  logout() {
    this.auth.logout();
  }
}
