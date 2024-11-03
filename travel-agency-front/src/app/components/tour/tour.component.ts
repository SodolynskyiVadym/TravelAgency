import { Component, OnInit } from '@angular/core';
import { Tour } from '../../../models/tour.model';
import { TourApiService } from '../../../services/api/tour-api.service';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { DateHelperService } from '../../../services/date-helper.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { UserEmailRole } from '../../../models/userEmailRole.model';
import { UserApiService } from '../../../services/api/user-api.service';
import { BrowserStorageService } from '../../../services/browser-storage-service.service';
import { PaymentApiService } from '../../../services/api/payment-api.service';
import { Place } from '../../../models/place.model';
import { Transport } from '../../../models/transport.model';
import { StripeService } from '../../../services/stripe.service';
import { ReviewApiService } from '../../../services/api/review-api.service';
import { Review } from '../../../models/review.model';

@Component({
  selector: 'app-tour',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterModule],
  templateUrl: './tour.component.html',
  styleUrls: [
    './tour.component.css',
  ]
})
export class TourComponent implements OnInit {
  tour: Tour = {
    id: 0,
    name: '',
    description: '',
    startDate: new Date(),
    formattedStartDate: '',
    endDate: new Date(),
    formattedEndDate: '',
    price: 0,
    quantitySeats: 0,
    imageUrl: '',
    isAvailable: true,
    placeStartId: 0,
    placeStart: {} as Place,
    placeEndId: 0,
    placeEnd: {} as Place,
    transportToEndId: 0,
    transportToEnd: {} as Transport,
    destinations: []
  }
  userReview = { id: 0, text: '', rating: 0, userId: 0, tourId: 0 } as Review;
  reviews = [] as Review[];
  stripe = {} as any;
  quantity = 1;
  freeSeats = 0;
  user: UserEmailRole = { email: '', role: '' , id: 0};
  isSending = false;
  hasUserPaid = true;
  isUserReview = false;

  constructor(private tourApi: TourApiService,
    private userApi: UserApiService,
    private route: ActivatedRoute,
    private router: Router,
    private dateHelper: DateHelperService,
    private payApi: PaymentApiService,
    private browserStorage: BrowserStorageService,
    private stripeService: StripeService,
    private reviewApi: ReviewApiService) { }

  async ngOnInit(): Promise<void> {
    const id = Number.parseInt(this.route.snapshot.paramMap.get('id')?.toString() || '0');
    if (id <= 0) {
      this.router.navigate(['/page-not-found']);
      return;
    }
    this.tour.id = id;
    this.tourApi.getTourById(id).subscribe((response: Tour | null) => {
      if (!response) {
        this.router.navigate(['/page-not-found']);
        return;
      }
      this.tour = response;
      this.tour.formattedStartDate = this.dateHelper.formatDate(this.tour.startDate);
      this.tour.formattedEndDate = this.dateHelper.formatDate(this.tour.endDate);
      this.tour.destinations.forEach(destination => {
        destination.formattedStartDate = this.dateHelper.formatDate(destination.startDate);
        destination.formattedEndDate = this.dateHelper.formatDate(destination.endDate);
      });
    });
    this.payApi.getTourFreeSeats(this.tour.id).subscribe({
      next: (response: number) => {
        this.freeSeats = response;
      },
      error: (error) => {
        this.freeSeats = 0;
      }
    });
    let token = this.browserStorage.get('token');
    if (token) {
      this.userApi.getUserByToken(token).subscribe({
        next: (response: UserEmailRole) => {
          this.user = response;
        },
        error: (error) => {
          this.browserStorage.remove('token');
          this.router.navigate(['/']);
        }
      });

      this.payApi.hasUserPaidForTour(id, token).subscribe({
        next: (response: boolean) => {
          this.hasUserPaid = response;
        },
        error: (error) => {
          this.hasUserPaid = false;
        }
      });

      this.reviewApi.getUserTourReview(id, token).subscribe({
        next: (response) => {
          if(response != null) this.userReview = response;
          this.isUserReview = response != null;
        },
        error: (error) => {
          this.userReview = { id: 0, text: '', rating: 0, userId: 0, tourId: 0 };
        }
      });

      this.reviewApi.getTourReviews(id).subscribe({
        next: (response: Review[]) => {
          this.reviews = response;
        },
        error: (error) => {
          this.reviews = [];
        }
      });

      this.stripe = await this.stripeService.getStripe();
    }
  }

  checkRating(){
    if(this.userReview.rating < 1) this.userReview.rating = 1;
    if(this.userReview.rating > 5) this.userReview.rating = 5;
  }

  reserveTour() {
    this.isSending = true;
    let token = this.browserStorage.get('token');
    const id = Number.parseInt(this.route.snapshot.paramMap.get('id')?.toString() || '0');
    if (!token) {
      this.user = {} as UserEmailRole;
      this.isSending = false;
      return;
    }
    if (id <= 0) {
      this.router.navigate(['/']);
      this.isSending = false;
      return;
    }
    this.payApi.reserveTour({ tourId: id, quantity: this.quantity }, token).subscribe({
      next: (response) => {
        console.log(response);
        this.stripe.redirectToCheckout({ sessionId: response.sessionId });
      },
      error: () => this.router.navigate(['/'])
    });
  }

  createReview(){
    let token = this.browserStorage.get('token');
    const id = Number.parseInt(this.route.snapshot.paramMap.get('id')?.toString() || '0');
    if (!token) {
      this.user = {} as UserEmailRole;
      return;
    }
    if (id <= 0) {
      this.router.navigate(['/']);
      return;
    }
    this.userReview.tourId = id;
    this.reviewApi.createReview(this.userReview, token).subscribe({
      next: (response) => {
        this.userReview.userId = this.user.id;
        this.isUserReview = true;
        this.reviews.push(this.userReview);
      },
      error: () => this.router.navigate(['/'])
    });
  }

  updateReview(){
    let token = this.browserStorage.get('token');
    const id = Number.parseInt(this.route.snapshot.paramMap.get('id')?.toString() || '0');
    if (!token) {
      this.user = {} as UserEmailRole;
      return;
    }
    if (id <= 0) {
      this.router.navigate(['/']);
      return;
    }
    this.userReview.tourId = id;
    this.reviewApi.updateReview(this.userReview, token).subscribe({
      next: (response) => {
        this.reviews = this.reviews.filter(review => review.userId != this.user.id)
        this.reviews.push(this.userReview);
        this.isUserReview = true;
      },
      error: () => this.router.navigate(['/'])
    });
  }

  deleteReview(){
    let token = this.browserStorage.get('token');
    const id = Number.parseInt(this.route.snapshot.paramMap.get('id')?.toString() || '0');
    if (!token) {
      this.user = {} as UserEmailRole;
      return;
    }
    if (id <= 0) {
      this.router.navigate(['/']);
      return;
    }
    this.reviewApi.deleteReview(id, token).subscribe({
      next: (response) => {
        this.isUserReview = false;
        this.reviews = this.reviews.filter(review => review.userId != this.user.id)
        this.userReview = { id: 0, text: '', rating: 0, userId: 0, tourId: 0 };
      },
      error: () => alert('Error deleting review')
    });
  }
}
