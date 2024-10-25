import { Component, OnInit } from '@angular/core';
import { Tour } from '../../../models/tour.model';
import { TourApiService } from '../../../services/api/tour/tour-api.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-main-page',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.css',
    '../../../../public//styles/style-input-search.css'
  ],
})

export class MainPageComponent implements OnInit {
  inputTourName : string = '';
  filteredTours : Tour[] = [];
  tours : Tour[] = []

  constructor(private tourService : TourApiService) { }

  ngOnInit(): void {
    this.tourService.getTours().subscribe((response : Tour[]) => {
      this.tours = response;
      this.filteredTours = this.tours;
    });
  }


  searchTour() {
    this.filteredTours = this.tours.filter(tour => tour.name.toLowerCase().includes(this.inputTourName.toLowerCase()));
  }
}
