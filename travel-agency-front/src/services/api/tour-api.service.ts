import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Tour } from '../../models/tour.model';
import { environment } from '../../environment/environment';
import { TourBasicDto } from '../../models/tourBasicDto.mode';

@Injectable({
  providedIn: 'root'
})
export class TourApiService {
  private apiUrl = `${environment.server}/tour`;

  constructor(private http : HttpClient) { }

  getTourById(id : number) : Observable<Tour> {
    return this.http.get<Tour>(`${this.apiUrl}/${id}`);
  }

  getTours() : Observable<Tour[]> {
    return this.http.get<Tour[]>(`${this.apiUrl}/getAllTours`);
  }

  getAvailableTours() : Observable<TourBasicDto[]> {
    return this.http.get<TourBasicDto[]>(`${this.apiUrl}/getAvailableTours`);
  }

  getUnavailableTours() : Observable<Tour[]> {
    return this.http.get<Tour[]>(`${this.apiUrl}/getUnavailableTours`);
  }

  createTour(tour : any, token : string) {
    return this.http.post(`${this.apiUrl}/create`, tour, { 
      observe: 'response', 
      headers : {
        'Authorization': `Bearer ${token}`
      }
    });
  }

  updateTour(tour : any, token : string){
    return this.http.patch(`${this.apiUrl}/update`, tour, { 
      observe: 'response', 
      headers : {
        'Authorization': `Bearer ${token}`
      }
    });
  }
}
