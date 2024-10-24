import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Hotel } from '../../models/hotel.model';
import { environment } from '../../environment/environment';

@Injectable({
  providedIn: 'root'
})
export class HotelApiService {
  private apiUrl = `${environment.server}/hotel`;

  constructor(private http: HttpClient) { }

  getHotels() : Observable<Hotel[]> {
    return this.http.get<Hotel[]>(`${this.apiUrl}/getAllHotels`);
  }
}
