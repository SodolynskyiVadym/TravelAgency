import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environment/environment';
import { Hotel } from '../../models/hotel.model';

@Injectable({
  providedIn: 'root'
})
export class HotelApiService {
  private apiUrl = `${environment.server}/hotel`;

  constructor(private http: HttpClient) { }

  getHotelById(id: string): Observable<Hotel | null> {
    return this.http.get<Hotel>(`${this.apiUrl}/${id}`);
  }

  getHotels(): Observable<Hotel[]> {
    return this.http.get<Hotel[]>(`${this.apiUrl}/getAllHotels`);
  }

  createHotel(hotel: Hotel, token: string): Observable<HttpResponse<Hotel>> {
    return this.http.post<Hotel>(`${this.apiUrl}/create`, hotel, {
      observe: 'response',
      headers: {
        'Authorization': `Bearer ${token}`
      }
    });
  }

  updateHotel(hotel: Hotel, token: string): Observable<HttpResponse<Hotel>> {

    return this.http.patch<Hotel>(`${this.apiUrl}/update`, hotel, {
      observe: 'response',
      headers: {
        'Authorization': `Bearer ${token}`
      }
    });
  }
}
