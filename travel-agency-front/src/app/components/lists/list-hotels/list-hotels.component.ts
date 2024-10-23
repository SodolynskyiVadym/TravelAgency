import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HotelApiService } from '../../../services/hotel/hotel-api.service';
import { Hotel } from '../../../models/hotel.model';

@Component({
  selector: 'app-list-hotels',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './list-hotels.component.html',
  styleUrls: [
    './list-hotels.component.css',
    '../../../../styles/style-table.css',
    '../../../../styles/style-input-search.css',
    '../../../../styles/style-button-create.css'
  ],
})

export class ListHotelsComponent implements OnInit {
  hotels: Hotel[] = [];
  constructor(private hotelApi : HotelApiService) { }

  ngOnInit(): void {
    this.hotelApi.getHotels().subscribe(
      (response: Hotel[]) => {
        this.hotels = response;
      },
      (error) => {
        console.error('Error fetching hotels', error);
      }
    );
  }

}
