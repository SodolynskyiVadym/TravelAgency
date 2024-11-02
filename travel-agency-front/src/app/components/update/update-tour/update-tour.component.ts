import { Component } from '@angular/core';
import { DestinationDto } from '../../../../models/destinationDto.model';
import { countries } from '../../../../services/constants/countries';
import { Transport } from '../../../../models/transport.model';
import { PlaceIdNameCountry } from '../../../../models/placeIdNameCountry.model';
import { HotelApiService } from '../../../../services/api/hotel-api.service';
import { TransportApiService } from '../../../../services/api/transport-api.service';
import { PlaceApiService } from '../../../../services/api/place-api.service';
import { TourApiService } from '../../../../services/api/tour-api.service';
import { ActivatedRoute, Router } from '@angular/router';
import { BrowserStorageService } from '../../../../services/browser-storage-service.service';
import { Tour } from '../../../../models/tour.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DateHelperService } from '../../../../services/date-helper.service';
import { ValidatorService } from '../../../../services/validator.service';

@Component({
  selector: 'app-update-tour',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './update-tour.component.html',
  styleUrls: [
    './update-tour.component.css',
    '../../../../assets/styles/style-form-create.css'
  ]
})
export class UpdateTourComponent {
  tour = {
    id: 0,
    description: '',
    destinations: [] as DestinationDto[],
    placeEndId: 0,
    imageUrl: '',
    name: '',
    price: 0,
    quantitySeats: 0,
    startDate: '',
    endDate: '',
    placeStartId: 0,
    transportToEndId: 0
  };
  quantityDays: number = 0;
  startPlaceName: string = '';
  endPlaceName: string = '';
  startPlaceCountry: string = '';
  endPlaceCountry: string = '';
  endTransportName: string = '';
  isCorrectImage: boolean = false;
  isCorrectInputs: boolean = false;
  isSending: boolean = false;
  countries: string[] = countries;
  destinationsCountries: string[] = [];
  destinationsPlacesNames: string[] = [];
  destinationsHotelsNames: string[] = [];
  destinationsTransportsNames: string[] = [];
  transports: Transport[] = [];
  places: PlaceIdNameCountry[] = [];
  hotels: any[] = [];

  constructor(
    private hotelApi: HotelApiService,
    private transportApi: TransportApiService,
    private placeApi: PlaceApiService,
    private tourApi: TourApiService,
    private router: Router,
    private route: ActivatedRoute,
    private browserStorage: BrowserStorageService,
    private dateHelper: DateHelperService,
    private validator: ValidatorService
  ) { }

  ngOnInit(): void {
    const id = Number.parseInt(this.route.snapshot.paramMap.get('id')?.toString() || '0');
    if (id <= 0) {
      this.router.navigate(['/page-not-found']);
      return;
    }
    this.tourApi.getTourById(id).subscribe(async (response: Tour | null) => {
      if (!response) {
        this.router.navigate(['/page-not-found']);
        return;
      }
      this.tour.id = response.id;
      this.tour.description = response.description;
      this.tour.endDate = this.dateHelper.formatDateForInput(response.endDate);
      this.tour.startDate = this.dateHelper.formatDateForInput(response.startDate);
      this.tour.imageUrl = response.imageUrl;
      this.tour.name = response.name;
      this.tour.placeEndId = response.placeEndId;
      this.tour.placeStartId = response.placeStartId;
      this.tour.price = response.price;
      this.tour.quantitySeats = response.quantitySeats;
      this.tour.transportToEndId = response.transportToEndId;
      for (let destination of response.destinations) {
        this.tour.destinations.push({
          startDate: this.dateHelper.formatDateForInput(destination.startDate),
          endDate: this.dateHelper.formatDateForInput(destination.endDate),
          placeId: destination.hotel.placeId,
          hotelId: destination.hotelId,
          transportId: destination.transportId
        });
        this.destinationsCountries.push(destination.hotel.place.country);
      }

      this.placeApi.getPlacesInfo().subscribe((places) => {
        this.places = places;
        for (let i = 0; i < this.tour.destinations.length; i++) this.destinationsPlacesNames.push(this.places.find((place) => place.id == this.tour.destinations[i].placeId)?.name || '');
      });

      this.hotelApi.getHotels().subscribe((hotels) => {
        this.hotels = hotels;
        for (let i = 0; i < this.tour.destinations.length; i++) this.destinationsHotelsNames.push(this.hotels.find((hotel) => hotel.id == this.tour.destinations[i].hotelId)?.name || '');
      });

      this.transportApi.getTransports().subscribe((transports) => {
        this.transports = transports;
        for (let i = 0; i < this.tour.destinations.length; i++) this.destinationsTransportsNames.push(this.transports.find((transport) => transport.id == this.tour.destinations[i].transportId)?.name || '');
      });
      this.startPlaceName = this.places.find((place) => place.id == this.tour.placeStartId)?.name || '';
      this.endPlaceName = this.places.find((place) => place.id == this.tour.placeEndId)?.name || '';
      this.startPlaceCountry = this.places.find((place) => place.id == this.tour.placeStartId)?.country || '';
      this.endPlaceCountry = this.places.find((place) => place.id == this.tour.placeEndId)?.country || '';
      this.endTransportName = this.transports.find((transport) => transport.id == this.tour.transportToEndId)?.name || '';
      this.isCorrectImage = await this.validator.checkImageExists(this.tour.imageUrl);
      this.checkInputs();
    });

  }

  rescheduleTour() {
    this.tour.startDate = this.dateHelper.addDaysToDateString(this.tour.startDate, this.quantityDays);
    this.tour.endDate = this.dateHelper.addDaysToDateString(this.tour.endDate, this.quantityDays);
    for (let i = 0; i < this.tour.destinations.length; i++) {
      this.tour.destinations[i].startDate = this.dateHelper.addDaysToDateString(this.tour.destinations[i].startDate, this.quantityDays);
      this.tour.destinations[i].endDate = this.dateHelper.addDaysToDateString(this.tour.destinations[i].endDate, this.quantityDays);
    }
    this.checkInputs();
  }


  getPlaceIdByNameAndCountry(placeName: string, country: string) {
    return this.places.find((place) => place.name == placeName && place.country == country)?.id || 0;
  }

  getHotelIdByHotelNameAndPlaceId(hotelName: string, placeId: number) {
    return this.hotels.find((hotel) => hotel.name == hotelName && hotel.placeId == placeId)?.id || 0;
  }

  getTransportIdByTransportName(transportName: string) {
    return this.transports.find((transport) => transport.name == transportName)?.id || 0;
  }

  filterPlacesByCountry(country: string) {
    return this.places.filter((place) => place.country == country);
  }

  filterHotelsByPlaceId(placeId: number) {
    return this.hotels.filter((hotel) => hotel.placeId == placeId);
  }

  isPlaceInCountry(index: number): boolean {
    let filteredPlaces = this.filterPlacesByCountry(this.destinationsCountries[index])
    return filteredPlaces.filter(place => place.name == this.destinationsPlacesNames[index]).length > 0;
  }

  checkInputs() {
    // console.log('Checking tour name:', this.tour.name != '');
    // console.log('Checking tour description:', this.tour.description != '');
    // console.log('Checking tour price:', this.tour.price > 0);
    // console.log('Checking image correctness:', this.isCorrectImage);
    // console.log('Checking tour quantity seats:', this.tour.quantitySeats > 0);
    // console.log('Checking tour start and end date:', this.tour.startDate < this.tour.endDate);
    // console.log('Checking tour dates:', this.checkTourDates());
    // console.log('Checking IDs:', this.checkIds());

    this.isCorrectInputs = this.tour.name != '' && this.tour.description != '' && this.tour.price > 0 && this.isCorrectImage && this.tour.quantitySeats > 0
      && this.tour.startDate < this.tour.endDate && this.checkTourDates() && this.checkIds();
  }

  compareToday(date: string): boolean {
    return new Date(date) < new Date();
  }

  checkIds(): boolean {
    let isValid = this.tour.placeStartId != 0 && this.tour.placeEndId != 0 && this.tour.transportToEndId != 0;
    for (let i = 0; i < this.tour.destinations.length; i++) {
      if (this.tour.destinations[i].placeId == 0 || this.tour.destinations[i].hotelId == 0 || this.tour.destinations[i].transportId == 0) {
        isValid = false;
        break;
      }
    }
    return isValid;
  }

  checkTourDates(): boolean {
    let isValid = true;
    for (let destination of this.tour.destinations) {
      if (destination.startDate >= destination.endDate) {
        console.log("Invalid destination dates");
        isValid = false;
        this.isCorrectInputs = false;
        break;
      }
    }
    if (this.tour.destinations.length == 0 || new Date(this.tour.startDate) > new Date(this.tour.destinations[0].startDate)
      || new Date(this.tour.endDate) < new Date(this.tour.destinations[this.tour.destinations.length - 1].endDate)
      || new Date(this.tour.startDate) < new Date()
    ) {
      console.log("Invalid tour dates");
      isValid = false;
      this.isCorrectInputs = false;
    }
    return isValid;
  }

  async checkImage() {
    this.isCorrectImage = true;
    this.checkInputs();
  }



  addDestination() {
    console.log('Destination added');
    this.tour.destinations.push({
      startDate: '',
      endDate: '',
      placeId: 0,
      hotelId: 0,
      transportId: 0
    });

    this.destinationsCountries.push('');
    this.destinationsPlacesNames.push('');
    this.destinationsHotelsNames.push('');
    this.destinationsTransportsNames.push('');
  }

  removeDestination(index: number) {
    this.tour.destinations.splice(index, 1);
    this.destinationsCountries.splice(index, 1);
    this.destinationsPlacesNames.splice(index, 1);
    this.destinationsHotelsNames.splice(index, 1);
    this.destinationsTransportsNames.splice(index, 1);
  }

  upadteTour() {
    this.isCorrectInputs = false;
    let token = this.browserStorage.get('token');
    if (token) {
      this.tourApi.updateTour(this.tour, token).subscribe({
        next: () => {
        },
        error: () => {
          alert("Tour was not updated. Please try again.");
        }
      })
    } else {
      this.router.navigate(['/login']);
    }
  }
}
