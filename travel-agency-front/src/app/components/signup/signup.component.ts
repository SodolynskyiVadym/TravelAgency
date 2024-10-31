import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { UserEmailPassword } from '../../../models/userEmailPassword.model';
import { UserApiService } from '../../../services/api/user-api.service';
import { BrowserStorageService } from '../../../services/browser-storage-service.service';
import { ValidatorService } from '../../../services/validator.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './signup.component.html',
  styleUrls: [
    './signup.component.css',
    '../../../assets/styles/style-login-signup.css'
  ]
})

export class SignupComponent {
  user : UserEmailPassword = { email: '', password: '' };
  confirmPassword : string = '';
  isSending : boolean = false;
  message : string = '';

  constructor(private userApi : UserApiService, private browserStorage : BrowserStorageService, private validator : ValidatorService, private router : Router) { }

  checkEmail(){
    return this.validator.checkEmail(this.user.email);
  }


  signup(){
    this.userApi.signup(this.user).subscribe({
      next: result => {
        this.browserStorage.set('token', result.token);
        this.router.navigate(['/']);
      },
      error: error => {
        this.message = "User wasn't created";
      }
    });
  }
}
