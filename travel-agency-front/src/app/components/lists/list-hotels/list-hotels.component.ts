import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HotelApiService } from '../../../../services/api/hotel-api.service';
import { Hotel } from '../../../../models/hotel.model';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { BrowserStorageService } from '../../../../services/browser-storage-service.service';

@Component({
  selector: 'app-list-hotels',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './list-hotels.component.html',
  styleUrls: [
    './list-hotels.component.css',
    '../../../../assets/styles/style-table.css',
    '../../../../assets/styles/style-input-search.css',
    '../../../../assets/styles/style-button-create.css'
  ],
})

export class ListHotelsComponent implements OnInit {
  inputHotelName: string = '';
  filteredHotels: Hotel[] = [];
  hotels: Hotel[] = [];
  usedHotelsIds: number[] = [];

  constructor(private hotelApi: HotelApiService, private router: Router, private browserStorage : BrowserStorageService) { }

  ngOnInit(): void {
    this.hotelApi.getHotels().subscribe(
      (response: Hotel[]) => {
        this.hotels = response;
        this.filteredHotels = this.hotels.sort((a, b) => a.place.country.localeCompare(b.place.country));
      },
      (error) => {
        console.error('Error fetching hotels', error);
      }
    );

    this.hotelApi.getUsedHotelsIds().subscribe({
      next: (response: number[]) => {
        this.usedHotelsIds = response;
      },
      error: (error) => {
        this.usedHotelsIds = this.hotels.map(hotel => hotel.id);
      }
    });
  }

  searchHotel() {
    this.filteredHotels = this.hotels.filter(hotel => hotel.name.toLowerCase().includes(this.inputHotelName.toLowerCase()));
  }

  deleteHotel(id: number) {
    let token = this.browserStorage.get('token');
    if(token == null) {
      this.router.navigate(['/login']);
      return;
    }
    this.hotelApi.deleteHotel(id, token).subscribe(
      (response) => {
        this.filteredHotels = this.filteredHotels.filter(hotel => hotel.id !== id);
      },
      (error) => {
        alert('Error deleting hotel');
      }
    );
  }
}
