import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Hotel } from '../../../models/hotel.model';
import { environment } from '../../../environment/environment';

@Injectable({
  providedIn: 'root'
})
export class HotelApiService {
  private apiUrl = `${environment.server}/hotel`;

  constructor(private http: HttpClient) { }

  getHotelById(id: string) : Observable<Hotel | null> {
    return this.http.get<Hotel>(`${this.apiUrl}/${id}`);
  }

  getHotels() : Observable<Hotel[]> {
    return this.http.get<Hotel[]>(`${this.apiUrl}/getAllHotels`);
  }

  updateHotel(hotel: Hotel): Observable<HttpResponse<Hotel>> {
    return this.http.patch<Hotel>(`${this.apiUrl}/update`, hotel, { observe: 'response' });
  }
}
