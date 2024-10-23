import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Hotel } from '../../models/hotel.model';

@Injectable({
  providedIn: 'root'
})
export class HotelApiService {

  private server = 'http://localhost:5160';
  private apiUrl = `${this.server}/hotels`;

  constructor(private http: HttpClient) { }

  getHotels() : Observable<Hotel[]> {
    return this.http.get<Hotel[]>(`${this.apiUrl}/getHotels`);
  }
}
