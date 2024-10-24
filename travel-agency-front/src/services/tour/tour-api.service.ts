import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Tour } from '../../models/tour.model';

@Injectable({
  providedIn: 'root'
})
export class TourApiService {
  private server = 'http://localhost:5160';
  private apiUrl = `${this.server}/tour`;

  constructor(private http : HttpClient) { }

  getTours() : Observable<Tour[]> {
    return this.http.get<Tour[]>(`${this.apiUrl}/getAllTours`);
  }
}
