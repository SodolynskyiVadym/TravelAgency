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

  getHotelById(id: number): Observable<Hotel | null> {
    return this.http.get<Hotel>(`${this.apiUrl}/${id}`);
  }

  getUsedHotelsIds(): Observable<number[]> {
    return this.http.get<number[]>(`${this.apiUrl}/getUsedHotelsIds`);
  }

  getHotels(): Observable<Hotel[]> {
    return this.http.get<Hotel[]>(`${this.apiUrl}/getAllHotels`);
  }

  createHotel(hotel: Hotel, token: string){
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

  deleteHotel(id: number, token: string): Observable<HttpResponse<any>> {
    return this.http.delete<any>(`${this.apiUrl}/delete/${id}`, {
      observe: 'response',
      headers: {
        'Authorization': `Bearer ${token}`
      }
    });
  }
}
