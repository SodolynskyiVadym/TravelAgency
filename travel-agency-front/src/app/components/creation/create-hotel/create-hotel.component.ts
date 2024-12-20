import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Hotel } from '../../../../models/hotel.model';
import { PlaceApiService } from '../../../../services/api/place-api.service';
import { countries } from '../../../../services/constants/countries';
import { ValidatorService } from '../../../../services/validator.service';
import { HotelApiService } from '../../../../services/api/hotel-api.service';
import { BrowserStorageService } from '../../../../services/browser-storage-service.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-hotel',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './create-hotel.component.html',
  styleUrls: [
    './create-hotel.component.css',
    '../../../../assets/styles/style-form-create.css'
  ]
})
export class CreateHotelComponent implements OnInit {
  hotel: Hotel = {} as Hotel;
  countries: string[] = countries
  places: any[] = [];
  filteredPlaces: any[] = [];
  country: string = '';
  placeName: string = '';
  isCorrectImage: boolean = false;
  isCorrectInputs: boolean = false;
  isSending: boolean = false;


  constructor(private placeApi: PlaceApiService,
    private validator: ValidatorService,
    private hotelApi: HotelApiService,
    private browserStorage: BrowserStorageService,
    private router: Router) { }


  ngOnInit(): void {
    this.placeApi.getPlacesInfo().subscribe(response => {
      this.places = response;
    });
  }

  checkInputs() {
    this.isCorrectInputs = this.hotel.name != '' && this.hotel.address != '' && this.hotel.description != ''
      && this.hotel.placeId != 0 && this.hotel.pricePerNight > 0 && this.isCorrectImage;
      console.log(this.isCorrectInputs);
  }

  async checkImage() {
    this.isCorrectImage = await this.validator.checkImageExists(this.hotel.imageUrl);
  }

  checkCountry() {
    this.filteredPlaces = this.places.filter(place => place.country === this.country);
    if (this.filteredPlaces.length == 0) {
      this.placeName = '';
    } else {
      this.checkPlace();
    }
  }

  checkPlace() {
    this.hotel.placeId = this.places.find(place => place.name == this.placeName && place.country === this.country)?.id || 0;
  }

  createHotel() {
    this.isSending = true;
    let token = this.browserStorage.get('token');
    if (token) {
      this.hotelApi.createHotel(this.hotel, token).subscribe(
        response => {
          this.isSending = false;
          this.router.navigate(['/hotels']);
        },
        error => {
          this.isSending = false;
          alert('Error creating hotel');
        }
      );
    }else this.router.navigate(['/login']);
}



}
