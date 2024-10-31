import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HotelApiService } from '../../../../services/api/hotel-api.service';
import { Hotel } from '../../../../models/hotel.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PlaceApiService } from '../../../../services/api/place-api.service';
import { ValidatorService } from '../../../../services/validator.service';
import { countries } from '../../../../services/constants/countries';
import { BrowserStorageService } from '../../../../services/browser-storage-service.service';

@Component({
  selector: 'app-update-hotel',
  templateUrl: './update-hotel.component.html',
  standalone: true,
  imports: [CommonModule, FormsModule],
  styleUrls: [
    './update-hotel.component.css',
    '../../../../assets/styles/style-form-create.css'
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
    private validator: ValidatorService,
    private browserStorage: BrowserStorageService
  ) { }

  async ngOnInit(): Promise<void> {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (!id) {
        this.router.navigate(['/page-not-found']);
        return
      }

      this.hotelApi.getHotelById(id).subscribe(async (response: Hotel | null) => {
        if (!response) {
          console.error('Hotel not found');
          this.router.navigate(['/page-not-found']);
          return;
        }

        this.hotel = response;
        this.placeAPI.getPlacesInfo().subscribe(async (response: any[]) => {
          this.places = response;
          this.placeName = await (this.places.find(place => place.id === this.hotel.placeId)?.name || '');
          this.country = await this.places.find(place => place.id === this.hotel.placeId)?.country || '';
          await this.checkImage();
          await this.checkCorrectInputs();
        });
      });
    });
  }

  async checkImage() {
    this.isCorrectImage = await this.validator.checkImageExists(this.hotel.imageUrl);
    this.checkCorrectInputs();
  }

  async checkPlace() {
    this.filteredPlaces = this.places.filter(place => place.name.toLowerCase().includes(this.placeName.toLowerCase()));
    this.hotel.placeId = this.places.find(place => place.name === name && place.country === this.country).id | 0;
    this.checkCorrectInputs();
  }

  async checkCorrectInputs() {
    this.isCorrectInputs = this.hotel.placeId != 0 && !!this.hotel.address && !!this.hotel.description && this.hotel.pricePerNight > 0
      && this.isCorrectImage && this.countries.includes(this.country);
  }

  updateHotel() {
    this.isSending = true;
    let token = this.browserStorage.get("token");
    if (token) {
      this.hotelApi.updateHotel(this.hotel, token).subscribe((response) => {
        if (response.status !== 200) {
          this.isSending = false;
          console.error('Error updating hotel');
          alert('Error updating hotel');
        } else {
          this.router.navigate(['/hotels']);
        }
      });
    }else{
      this.isSending = false;
      this.router.navigate(['/login']);
    }

  }
}