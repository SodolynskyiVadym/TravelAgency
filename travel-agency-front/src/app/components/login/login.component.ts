import { Component } from '@angular/core';
import { ValidatorService } from '../../../services/validator.service';
import { UserApiService } from '../../../services/api/user-api.service';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BrowserStorageService } from '../../../services/browser-storage-service.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrls: [
    './login.component.css',
    '../../../assets/styles/style-login-signup.css'
  ]
})
export class LoginComponent {
  email: string = '';
  password: string = '';
  isSending: boolean = false;
  isCorrectInputs: boolean = false;

  constructor(private validator : ValidatorService, private userApi : UserApiService, private router : Router, private browserStorage : BrowserStorageService) { }

  checkInputs(){
    this.isCorrectInputs = this.validator.checkEmail(this.email) && this.password.length > 7;
  }

  login() {
    this.isSending = true;
    if(this.isCorrectInputs){
      this.userApi.login({email : this.email, password : this.password}).subscribe({
        next: result => {
          this.browserStorage.set('token', result.token);
          this.router.navigate(['/']);
        },
        error: error => {
          console.log('error');
          alert('Incorrect email or password');
        }
      });
    }
    this.isSending = false;
  }
}
