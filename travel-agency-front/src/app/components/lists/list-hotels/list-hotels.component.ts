import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HotelApiService } from '../../../../services/api/hotel/hotel-api.service';
import { Hotel } from '../../../../models/hotel.model';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-list-hotels',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './list-hotels.component.html',
  styleUrls: [
    './list-hotels.component.css',
    '../../../../../public/styles/style-table.css',
    '../../../../../public/styles/style-input-search.css',
    '../../../../../public/styles/style-button-create.css'
  ],
})

export class ListHotelsComponent implements OnInit {
  inputHotelName : string = '';
  filteredHotels: Hotel[] = [];
  hotels: Hotel[] = [];
  
  constructor(private hotelApi : HotelApiService, private router : Router) { }

  ngOnInit(): void {
    this.hotelApi.getHotels().subscribe(
      (response: Hotel[]) => {
        this.hotels = response;
        this.filteredHotels = this.hotels;
      },
      (error) => {
        console.error('Error fetching hotels', error);
      }
    );
  }

  searchHotel() {
    this.filteredHotels = this.hotels.filter(hotel => hotel.name.toLowerCase().includes(this.inputHotelName.toLowerCase()));
  }
}
