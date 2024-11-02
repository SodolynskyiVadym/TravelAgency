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
    destinations : []
  }
  quantity = 1;
  freeSeats = 0;
  user: UserEmailRole = { email: '', role: '' };
  isSending = false;
  hasUserPaid = true;

  constructor(private tourApi: TourApiService,
    private userApi: UserApiService,
    private route: ActivatedRoute,
    private router: Router,
    private dateHelper: DateHelperService,
    private payApi: PaymentApiService,
    private browserStorage: BrowserStorageService) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (!id) {
        this.router.navigate(['/page-not-found']);
        return;
      }
      this.tour.id = parseInt(id, 10);
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
      }
    });
  }

  reserveTour() {
    this.isSending = true;
    let token = this.browserStorage.get('token');
    if (!token) {
      this.user = { email: '', role: '' };
      return;
    }
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (!id) {
        this.router.navigate(['/page-not-found']);
        return;
      }
      this.payApi.reserveTour({ tourId: id, quantity: this.quantity }, token).subscribe({
        next: () => {
          this.router.navigate(['/user']);
        },
        error: () => {
          this.router.navigate(['/']);
        }
      });
    });
    this.isSending = false;
  }
}
