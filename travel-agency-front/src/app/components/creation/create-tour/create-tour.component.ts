import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { countries } from '../../../../services/constants/countries';
import { Transport } from '../../../../models/transport.model';
import { HotelApiService } from '../../../../services/api/hotel-api.service';
import { TransportApiService } from '../../../../services/api/transport-api.service';
import { PlaceApiService } from '../../../../services/api/place-api.service';
import { TourApiService } from '../../../../services/api/tour-api.service';



@Component({
  selector: 'app-create-tour',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './create-tour.component.html',
  styleUrls: [
    './create-tour.component.css',
    '../../../../assets/styles/style-form-create.css'
  ]
})

export class CreateTourComponent implements OnInit {
  tour = {
    description: '',
    destinations: [
      {
        startDate: new Date(),
        endDate: new Date(),
        placeId: 0,
        hotelId: 0,
        transportId: 0
      }
    ],
    placeEndId: 0,
    imageUrl: '',
    name: '',
    price: 0,
    quantitySeats: 0,
    startDate: new Date(),
    endDate: new Date(),
    placeStartId: 0,
    transportToEndId: 0
  };
  startPlaceName: string = '';
  endPlaceName: string = '';
  startPlaceCountry: string = '';
  endPlaceCountry: string = '';
  endTransportName: string = '';
  isCorrectImage: boolean = false;
  isCorrectInputs: boolean = false;
  isSending: boolean = false;
  countries: string[] = countries;
  destinationsCountries: string[] = [''];
  destinationsPlacesNames: string[] = [''];
  destinationsHotelsNames: string[] = [''];
  destinationsTransportsNames: string[] = [''];
  transports: Transport[] = [];
  places: any[] = [];
  hotels: any[] = [];

  constructor(private hotelApi: HotelApiService, private transportApi: TransportApiService, private placeApi: PlaceApiService, private tourApi : TourApiService) { }

  ngOnInit(): void {
    this.placeApi.getPlacesInfo().subscribe((places) => {
      this.places = places;
    });

    this.hotelApi.getHotels().subscribe((hotels) => {
      this.hotels = hotels;
    });

    this.transportApi.getTransports().subscribe((transports) => {
      this.transports = transports;
    });
  }


  getPlaceIdByNameAndCountry(placeName : string, country : string) {
    return this.places.find((place) => place.name == placeName && place.country == country)?.id || 0;
  }

  getHotelIdByHotelNameAndPlaceId(hotelName : string, placeId : number) {
    return this.hotels.find((hotel) => hotel.name == hotelName && hotel.placeId == placeId)?.id || 0;
  }

  getTransportIdByTransportName(transportName : string) {
    return this.transports.find((transport) => transport.name == transportName)?.id || 0;
  }

  filterPlacesByCountry(country : string){
    return this.places.filter((place) => place.country == country);
  }

  filterHotelsByPlaceId(placeId : number){
    return this.hotels.filter((hotel) => hotel.placeId == placeId);
  }

  checkInputs() {
    this.isCorrectInputs = this.tour.name != '' && this.tour.description != '' && this.tour.price > 0 && this.isCorrectImage;
  }

  compareToday(date : Date){
    return date < new Date();
  }
  
  checkTourDates() : boolean{
    let isValid = true;
    for (let destination of this.tour.destinations) {
      if (destination.startDate > destination.endDate) {
        isValid = false;
        this.isCorrectInputs = false;
        break;
      }
    }
    if(this.tour.destinations.length != 0 && (this.tour.startDate > this.tour.destinations[0].startDate || this.tour.endDate < this.tour.destinations[this.tour.destinations.length - 1].endDate)){
      isValid = false;
    }
    return isValid;
  }

  async checkImage() {
    this.isCorrectImage = true;
  }



  addDestination() {
    console.log('Destination added');
    this.tour.destinations.push({
      startDate: new Date(),
      endDate: new Date(),
      placeId: 0,
      hotelId: 0,
      transportId: 0
    });

    this.destinationsCountries.push('');
    this.destinationsPlacesNames.push('');
    this.destinationsHotelsNames.push('');
    this.destinationsTransportsNames.push('');
  }

  removeDestination(index : number) {
    this.tour.destinations.splice(index, 1);
    this.destinationsCountries.splice(index, 1);
    this.destinationsPlacesNames.splice(index, 1);
    this.destinationsHotelsNames.splice(index, 1);
    this.destinationsTransportsNames.splice(index, 1);
  }

  createTour() {
    this.tourApi.createTour(this.tour).subscribe((response) => {
      console.log(response);
    });
  }
}