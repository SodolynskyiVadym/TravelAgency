import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Place } from '../../../models/place.model';
import { environment } from '../../../environment/environment';

@Injectable({
  providedIn: 'root'
})
export class PlaceApiService {
  private apiUrl : string= `${environment.server}/place`;

  constructor(private http : HttpClient) { }

  getPlaceById(id : string) : Observable<Place | null>{
    return this.http.get<Place>(`${this.apiUrl}/getPlaceById/${id}`);
  }

  getPlaces() : Observable<Place[]>{
    return this.http.get<Place[]>(`${this.apiUrl}/getAllPlaces`);
  }

  getPlacesInfo() : Observable<any[]>{
    return this.http.get<any[]>(`${this.apiUrl}/getPlacesInfo`);
  }
}
