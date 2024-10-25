import { Component, OnInit } from '@angular/core';
import { Tour } from '../../../../models/tour.model';
import { TourApiService } from '../../../../services/api/tour/tour-api.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-list-unavailable-tours',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './list-unavailable-tours.component.html',
  styleUrls: [
    './list-unavailable-tours.component.css',
    '../../../../../public/styles/style-table.css',
    '../../../../../public/styles/style-input-search.css',
    '../../../../../public/styles/style-button-create.css'
  ]
})
export class ListUnavailableToursComponent implements OnInit {
  tours: Tour[] = [];
  filteredTours: Tour[] = [];
  inputTourName: string = '';

  constructor(private tourApi: TourApiService) { }

  ngOnInit(): void {
    this.tourApi.getUnavailableTours().subscribe((response: Tour[]) => {
      this.tours = response;
      this.filteredTours = this.tours;
    });
  }

  searchTour() {
    this.filteredTours = this.tours.filter(tour => tour.name.toLowerCase().includes(this.inputTourName.toLowerCase()));
  }
}
