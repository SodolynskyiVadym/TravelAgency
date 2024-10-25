import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Tour } from '../../../models/tour.model';
import { environment } from '../../../environment/environment';

@Injectable({
  providedIn: 'root'
})
export class TourApiService {
  private apiUrl = `${environment.server}/tour`;

  constructor(private http : HttpClient) { }

  getTourById(id : string) : Observable<Tour> {
    return this.http.get<Tour>(`${this.apiUrl}/${id}`);
  }

  getTours() : Observable<Tour[]> {
    return this.http.get<Tour[]>(`${this.apiUrl}/getAllTours`);
  }

  getUnavailableTours() : Observable<Tour[]> {
    return this.http.get<Tour[]>(`${this.apiUrl}/getUnavailableTours`);
  }
}
