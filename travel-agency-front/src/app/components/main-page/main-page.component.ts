import { Component, OnInit } from '@angular/core';
import { TourApiService } from '../../../services/api/tour-api.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { DateHelperService } from '../../../services/date-helper.service';
import { TourBasicDto } from '../../../models/tourBasicDto.mode';
import { PlaceApiService } from '../../../services/api/place-api.service';
import { countries } from '../../../services/constants/countries';

@Component({
  selector: 'app-main-page',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.css',
    '../../../assets/styles/style-input-search.css'
  ],
})

export class MainPageComponent implements OnInit {
  inputTourName: string = '';
  countries = countries;
  filteredTours: TourBasicDto[] = [];
  tours: TourBasicDto[] = []
  places: any[] = [];

  choosedStartDateTo: Date = new Date();
  choosedStartDateFrom: Date = new Date();
  choosedEndDateTo: Date = new Date();
  choosedEndDateFrom: Date = new Date();
  priceFrom: number = 0;
  priceTo: number = 0;
  choosedTransportTypes = [];
  choosedCountries = [];
  choosedPlaces = [];
  isFiltering: boolean = false;

  constructor(private tourService: TourApiService, private placeAPi : PlaceApiService, private dateHelper: DateHelperService) { }

  ngOnInit(): void {
    this.tourService.getAvailableTours().subscribe({
      next: (response: TourBasicDto[]) => {
        this.tours = response;
        this.tours.forEach(tour => {
          tour.formattedStartDate = this.dateHelper.formatDate(tour.startDate);
          tour.formattedEndDate = this.dateHelper.formatDate(tour.endDate);
        });
        this.filteredTours = this.tours;
      },error : (error) => {
        this.filteredTours = [];
        this.tours = [];
      }
    });

    this.placeAPi.getPlacesInfo().subscribe({
      next: (response: any) => {
        this.places = response;
    }, error: (error) => {
    }
    });
  }


  searchTour() {
    this.filteredTours = this.tours.filter(tour => tour.name.toLowerCase().includes(this.inputTourName.toLowerCase()));
  }
}
