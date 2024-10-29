import { Component } from '@angular/core';
import { ValidatorService } from '../../../services/validator/validator.service';
import { UserApiService } from '../../../services/api/user/user-api.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
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

  constructor(private validator : ValidatorService, private userApi : UserApiService, private router : Router) { }

  checkInputs(){
    this.isCorrectInputs = this.validator.checkEmail(this.email) && this.password.length > 7;
  }

  login() {
    this.isSending = true;
    if(this.isCorrectInputs){
      this.userApi.login({email : this.email, password : this.password}).subscribe({
        next: result => {
          localStorage.setItem('token', result.token);
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
