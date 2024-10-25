import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HotelApiService } from '../../../../services/api/hotel/hotel-api.service';
import { Hotel } from '../../../../models/hotel.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PlaceApiService } from '../../../../services/api/place/place-api.service';
import { ValidatorService } from '../../../../services/validator/validator.service';
import { countries } from '../../../../services/constants/countries';

@Component({
  selector: 'app-update-hotel',
  templateUrl: './update-hotel.component.html',
  standalone: true,
  imports: [CommonModule, FormsModule],
  styleUrls: [
    './update-hotel.component.css',
    '../../../../../public/styles/style-form-create.css'
  ]
})
export class UpdateHotelComponent implements OnInit {
  hotel: Hotel = {} as Hotel;
  places: any[] = [];
  placeName: string = '';
  country: string = '';
  countries: string[] = countries;
  isCorrectInputs: boolean = false;
  isCorrectImage: boolean = false;
  isSending: boolean = false;
  filteredPlaces: any[] = [];

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private hotelApi: HotelApiService,
    private placeAPI: PlaceApiService,
    private validator: ValidatorService
  ) { }

  async ngOnInit(): Promise<void> {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.hotelApi.getHotelById(id).subscribe((response: Hotel | null) => {
          if (!response) {
            console.error('Hotel not found');
            this.router.navigate(['/page-not-found']);
            return;
          } else {
            this.hotel = response;
          }
        });
      }
    });

    this.placeAPI.getPlacesInfo().subscribe((response: any[]) => {
      this.places = response;
    });

    this.placeName = this.places.find(place => place.id === this.hotel.placeId)?.name || '';
    this.country = this.places.find(place => place.id === this.hotel.placeId)?.country || '';

    await this.checkImage();
    await this.checkCorrectInputs();
  }

  async checkImage() {
    this.isCorrectImage = await this.validator.checkImageExists(this.hotel.imageUrl);
    await this.checkCorrectInputs();
  }

  async checkPlace() {
    this.filteredPlaces = this.places.filter(place => place.name.toLowerCase().includes(this.placeName.toLowerCase()));
    this.hotel.placeId = this.places.find(place => place.name === name && place.country === this.country).id | 0;
    await this.checkCorrectInputs();
  }

  async checkCorrectInputs() {
    this.isCorrectInputs = this.hotel.placeId != 0 && !!this.hotel.address && !!this.hotel.description && this.hotel.pricePerNight > 0
      && this.isCorrectImage && this.countries.includes(this.country);  
  }

  async updateHotel() {
    this.isSending = true;
    this.hotelApi.updateHotel(this.hotel).subscribe((response) => {
      console.log(response);
      if (response.status !== 200) {
        this.isSending = false;
        console.error('Error updating hotel');
        alert('Error updating hotel');
      } else {
        this.router.navigate(['/hotels']);
      }
    });
  }
}