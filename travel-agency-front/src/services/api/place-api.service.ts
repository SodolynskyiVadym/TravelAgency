import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environment/environment';
import { Place } from '../../models/place.model';
import { PlaceIdNameCountry } from '../../models/placeIdNameCountry.model';
import { PlaceDto } from '../../models/placeDto.model';

@Injectable({
  providedIn: 'root'
})
export class PlaceApiService {
  private apiUrl : string= `${environment.server}/place`;

  constructor(private http : HttpClient) { }

  getPlaceById(id : number) : Observable<PlaceDto | null>{
    return this.http.get<PlaceDto>(`${this.apiUrl}/${id}`);
  }

  getPlaces() : Observable<Place[]>{
    return this.http.get<Place[]>(`${this.apiUrl}/getAllPlaces`);
  }

  getPlacesInfo() : Observable<PlaceIdNameCountry[]>{
    return this.http.get<PlaceIdNameCountry[]>(`${this.apiUrl}/getPlacesInfo`);
  }

  createPlace(place : Place, token : string) : Observable<any>{
    return this.http.post<any>(`${this.apiUrl}/create`, place, {
      headers: {
        'Authorization': `Bearer ${token}`
      }
    });
  }

  updatePlace(place : PlaceDto, token : string) : Observable<any>{
    return this.http.patch<any>(`${this.apiUrl}/update`, place, {
      headers: {
        'Authorization': `Bearer ${token}`
      }
    });
  }
}
