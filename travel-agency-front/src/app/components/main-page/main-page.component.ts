import { Component, OnInit } from '@angular/core';
import { TourApiService } from '../../../services/api/tour-api.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { DateHelperService } from '../../../services/date-helper.service';
import { TourBasicDto } from '../../../models/tourBasicDto.mode';
import { PlaceApiService } from '../../../services/api/place-api.service';
import { countries } from '../../../services/constants/countries';
import { PlaceIdNameCountry } from '../../../models/placeIdNameCountry.model';

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
  places: PlaceIdNameCountry[] = [];

  choosedStartDateTo: string = '';
  choosedStartDateFrom: string = '';
  choosedEndDateTo: string = '';
  choosedEndDateFrom: string = '';
  priceFrom: number = 0;
  priceTo: number = 0;
  priceMax: number = 0;
  isShipIncluded: boolean = true;
  isAirplaneIncluded: boolean = true;
  isBusIncluded: boolean = true;
  isTrainIncluded: boolean = true;
  choosedCountries: string[] = [];
  choosedPlaces: string[] = [];
  isFiltering: boolean = false;

  constructor(private tourService: TourApiService, private placeAPi: PlaceApiService, private dateHelper: DateHelperService) { }

  ngOnInit(): void {
    this.choosedStartDateTo = this.dateHelper.formatDateForInput(new Date(new Date().setFullYear(new Date().getFullYear() + 1)));
    this.choosedStartDateFrom = this.dateHelper.formatDateForInput(new Date());
    this.choosedEndDateTo = this.dateHelper.formatDateForInput(new Date(new Date().setFullYear(new Date().getFullYear() + 1)));
    this.choosedEndDateFrom = this.dateHelper.formatDateForInput(new Date());

    this.tourService.getAvailableTours().subscribe({
      next: (response: TourBasicDto[]) => {
        this.tours = response;
        this.tours.forEach(tour => {
          tour.formattedStartDate = this.dateHelper.formatDate(tour.startDate);
          tour.formattedEndDate = this.dateHelper.formatDate(tour.endDate);
        });
        this.priceMax = Math.max(...this.tours.map(tour => tour.price));
        this.priceTo = this.priceMax
        this.filteredTours = this.tours;
      }, error: (error) => {
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

  filterPlaces(country: string): PlaceIdNameCountry[] {
    return this.places.filter(p => p.country == country);
  }


  searchTour() {
    this.filteredTours = this.tours.filter(tour => tour.name.toLowerCase().includes(this.inputTourName.toLowerCase()));
  }

  applyFilters() {
    let placesIds: number[] = []
    for (let i = 0; i < this.choosedCountries.length; i++)placesIds[i] = this.places.find(p => p.name == this.choosedPlaces[i] && p.country == this.choosedCountries[i])?.id || 0;
    
    for (let t of this.tours) {
      console.log(t.name)
      console.log(`End date is after start date filter: ${new Date(t.endDate) >= new Date(this.choosedEndDateFrom)}`);
      console.log(`End date is before end date filter: ${new Date(t.endDate) <= new Date(this.choosedEndDateTo)}`);
      console.log(`Start date is after start date filter: ${new Date(t.startDate) >= new Date(this.choosedStartDateFrom)}`);
      console.log(`Start date is before end date filter: ${new Date(t.startDate) <= new Date(this.choosedStartDateTo)}`);
      console.log(new Date(this.choosedEndDateFrom))
      console.log(new Date(this.choosedEndDateTo))
      console.log(new Date(this.choosedStartDateTo))
      console.log(new Date(this.choosedStartDateFrom))

      // console.log(`Price is above minimum filter: ${t.price >= this.priceFrom}`);
      // console.log(`Price is below maximum filter: ${t.price <= this.priceTo}`);
      // console.log(`Tour includes all selected places: ${placesIds.every(id => t.placeIds.includes(id))}`);
      // console.log(`Tour includes ship transport: ${(this.isShipIncluded || t.transportTypes.includes("SHIP") == false)}`);
      // console.log(`Tour includes airplane transport: ${this.isAirplaneIncluded || t.transportTypes.includes("AIRPLANE") == this.isAirplaneIncluded}`);
      // console.log(`Tour includes bus transport: ${this.isBusIncluded || t.transportTypes.includes("BUS") == this.isBusIncluded}`);
      // console.log(`Tour includes train transport: ${this.isTrainIncluded || t.transportTypes.includes("TRAIN") == this.isTrainIncluded}`);
    }

    this.filteredTours = this.tours.filter(t => 
      new Date(t.endDate) >= new Date(this.choosedEndDateFrom) &&
      new Date(t.endDate) <= new Date(this.choosedEndDateTo) &&
      new Date(t.startDate) >= new Date(this.choosedStartDateFrom) &&
      new Date(t.startDate) <= new Date(this.choosedStartDateTo) &&
      t.price >= this.priceFrom &&
      t.price <= this.priceTo &&
      placesIds.every(id => t.placeIds.includes(id)) &&
      (this.isShipIncluded || t.transportTypes.includes("SHIP") == false) &&
      (this.isAirplaneIncluded || t.transportTypes.includes("AIRPLANE") == this.isAirplaneIncluded) &&
      (this.isBusIncluded || t.transportTypes.includes("BUS") == this.isBusIncluded) &&
      (this.isTrainIncluded || t.transportTypes.includes("TRAIN") == this.isTrainIncluded)
    );
  }
}
