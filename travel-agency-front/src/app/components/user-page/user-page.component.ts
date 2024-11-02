import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { BrowserStorageService } from '../../../services/browser-storage-service.service';
import { UserApiService } from '../../../services/api/user-api.service';
import { UserEmailRole } from '../../../models/userEmailRole.model';
import { Payment } from '../../../models/payment.model';
import { PaymentApiService } from '../../../services/api/payment-api.service';
import { StripeService } from '../../../services/stripe.service';
import { DateHelperService } from '../../../services/date-helper.service';

@Component({
  selector: 'app-user-page',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterModule],
  templateUrl: './user-page.component.html',
  styleUrls: [
    './user-page.component.css',
    '../../../assets/styles/style-table.css',
    '../../../assets/styles/style-login-signup.css',
    '../../../assets/styles/style-button-create.css'
  ],
})
export class UserPageComponent implements OnInit {
  user: UserEmailRole = {} as UserEmailRole;
  payments = [] as Payment[];
  stripe: any;
  password = '';
  confirmPassword = '';

  constructor(private browserStorage: BrowserStorageService,
    private userApi: UserApiService,
    private router: Router,
    private paymentApi: PaymentApiService,
    private stipeService: StripeService,
    private dateHelper: DateHelperService) { }

  async ngOnInit(): Promise<void> {
    let token = this.browserStorage.get("token");
    if (token == null) this.router.navigate(['/login']);
    else {
      this.stripe = await this.stipeService.getStripe();
      this.userApi.getUserByToken(token).subscribe({
        next: (response) => {
          this.user = response;
        }, error: (err) => {
          this.browserStorage.remove("token");
          this.router.navigate(['/login']);
        }
      });

      this.paymentApi.getUserPayments(token).subscribe({
        next: (response) => {
          this.payments = response;
          this.payments.forEach((payment) => {
            payment.tour.formattedStartDate = this.dateHelper.formatDate(payment.tour.startDate);
            payment.tour.formattedEndDate = this.dateHelper.formatDate(payment.tour.endDate);
          });
        }, error: (err) => {
          this.payments = [];
        }
      });
    }
  }

  updatePassword() {
    let token = this.browserStorage.get("token");
    if (token == null) this.router.navigate(['/login']);
    else {
      this.userApi.updatePassword(this.password, token).subscribe({
        next: () => {
          this.password = '';
          this.confirmPassword = '';
        }, error: () => {
          alert("Password wasn't updated. Please try again.");
        }
      });
    }
  }

  payTour(stripeSessionId: string) {
    console.log(stripeSessionId);
    this.stripe.redirectToCheckout({ sessionId: stripeSessionId });
  }
}

