import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { UserApiService } from '../../../services/api/user-api.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { UserEmailRole } from '../../../models/userEmailRole.model';
import { AuthService } from '../../../services/auth.service';
import { BrowserStorageService } from '../../../services/browser-storage-service.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterModule, FormsModule, CommonModule],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})

export class HeaderComponent implements OnInit {
  user: UserEmailRole = { email: '', role: '' };
  token: string | null = null;

  constructor(private userApi: UserApiService, private auth : AuthService, private browserStorage : BrowserStorageService) { }

  ngOnInit(): void {
    let token = this.browserStorage.get("token");
    if(token != null && token != ""){
      this.userApi.getUserByToken(token).subscribe({
        next: (response) => {
          this.user.role = response.role;
          this.user.email = response.email;
        },
        error: (error) => {
          console.error("Error in header.component");
          this.browserStorage.remove("token");
        }
      });
    }
  }

  logout() {
    this.auth.logout();
  }
}
