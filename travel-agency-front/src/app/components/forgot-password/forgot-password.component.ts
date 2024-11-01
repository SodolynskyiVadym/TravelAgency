import { Component } from '@angular/core';
import { UserApiService } from '../../../services/api/user-api.service';
import { UserEmailPassword } from '../../../models/userEmailPassword.model';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ValidatorService } from '../../../services/validator.service';
import { BrowserStorageService } from '../../../services/browser-storage-service.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './forgot-password.component.html',
  styleUrls: [
    './forgot-password.component.css',
    '../../../assets/styles/style-login-signup.css'
  ]
})

export class ForgotPasswordComponent {
  user: UserEmailPassword = { email: '', password: '' };
  isPasswordCreated = false;
  isSending = false;

  constructor(private userApi: UserApiService, private validator: ValidatorService, private browserStorage: BrowserStorageService, private router: Router) { }

  checkEmail() {
    return this.validator.checkEmail(this.user.email);
  }

  createReservePassword() {
    this.isSending = true;
    this.userApi.createReservePassword(this.user.email).subscribe({
      next: (response) => {
        this.isPasswordCreated = true;
        this.isSending = false;

      }, error: (err) => {
        console.log(err);
        alert("Password wasn't created. Please try again.");
        this.isSending = false;

      }
    });
  }

  loginViaReservePassword() {
    this.isSending = true;
    this.userApi.loginViaReservePassword(this.user).subscribe({
      next: (response) => {
        this.browserStorage.set("token", response.token);
        this.router.navigate(['/']);
        this.isSending = false;
      }, error: () => {
        alert("Invalid password or request. Please try again.");
        this.isSending = false;
      }
    });
  }
}
