import { Component, OnInit } from '@angular/core';
import { Tour } from '../../../models/tour.model';
import { TourApiService } from '../../../services/api/tour/tour-api.service';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { DateHelperService } from '../../../services/date/date-helper.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-tour',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './tour.component.html',
  styleUrls: [
    './tour.component.css',
  ]
})
export class TourComponent implements OnInit {
  tour: Tour = {} as Tour;
  quantity = 1;

  constructor(private tourApi: TourApiService, private route: ActivatedRoute, private router: Router, private dateHelper: DateHelperService) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (!id) {
        this.router.navigate(['/page-not-found']);
        return;
      }
      this.tourApi.getTourById(id).subscribe((response: Tour | null) => {
        if (!response) {
          console.error('Tour not found');
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
        console.log("---------------------------------------")
        console.log(this.tour.placeStart.name);
      });
    });
  }

  calculatePrice(): number {
    return this.quantity * this.tour.price;
  }
}
