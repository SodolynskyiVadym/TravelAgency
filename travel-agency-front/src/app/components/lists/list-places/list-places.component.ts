import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { Place } from '../../../../models/place.model';
import { PlaceApiService } from '../../../../services/api/place-api.service';
import { BrowserStorageService } from '../../../../services/browser-storage-service.service';

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
  usedPlacesIds: number[] = [];

  constructor(private placeApi: PlaceApiService, private browserStorage: BrowserStorageService, private router: Router) { }


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

    this.placeApi.getUsedPlacesIds().subscribe({
      next: (response: number[]) => {
        this.usedPlacesIds = response;
      },
      error: (error) => {
        this.usedPlacesIds = this.places.map(place => place.id);
      }
    });
  }

  searchPlace() {
    this.filteredPlaces = this.places.filter(place => place.name.toLowerCase().includes(this.inputPlaceName.toLowerCase()));
  }

  deletePlace(id: number) {
    let token = this.browserStorage.get('token');
    if (token == null) {
      this.router.navigate(['/login']);
      return;
    }
    this.placeApi.deletePlace(id, token).subscribe({
      next: (response) => {
        this.places = this.places.filter(place => place.id !== id);
        this.filteredPlaces = this.filteredPlaces.filter(place => place.id !== id);
      },
      error: (error) => {
        alert('Error deleting place');
      }
    });
  }
}
