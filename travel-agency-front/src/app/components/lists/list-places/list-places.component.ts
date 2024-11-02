import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { Place } from '../../../../models/place.model';
import { PlaceApiService } from '../../../../services/api/place-api.service';

@Component({
  selector: 'app-list-places',
  standalone: true,
  imports: [RouterModule, CommonModule, FormsModule],
  templateUrl: './list-places.component.html',
  styleUrls: [
    './list-places.component.css',
    '../../../../assets/styles/style-table.css',
    '../../../../assets/styles/style-input-search.css',
    '../../../../assets/styles/style-button-create.css'
  ]
})


export class ListPlacesComponent implements OnInit {
  places: Place[] = [];
  filteredPlaces: Place[] = [];
  inputPlaceName: string = '';

  constructor(private placeApi: PlaceApiService) { }


  ngOnInit(): void {
    this.placeApi.getPlaces().subscribe(
      (response: Place[]) => {
        this.places = response.sort((a, b) => a.country.localeCompare(b.country));
        this.filteredPlaces = this.places;
      },
      (error) => {
        console.error('Error fetching places', error);
      }
    );
  }

  searchPlace() {
    this.filteredPlaces = this.places.filter(place => place.name.toLowerCase().includes(this.inputPlaceName.toLowerCase()));
  }
}
