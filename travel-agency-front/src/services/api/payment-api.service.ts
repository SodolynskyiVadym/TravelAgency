import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environment/environment';
import { Observable } from 'rxjs';
import { Payment } from '../../models/payment.model';

@Injectable({
  providedIn: 'root'
})
export class PaymentApiService {
  private apiUrl: string = `${environment.server}/pay`;

  constructor(private http: HttpClient) { }

  getUserPayments(token: string): Observable<Payment[]> {
    return this.http.get<Payment[]>(`${this.apiUrl}/getUserPayments`, {
      headers: {
        'Authorization': `Bearer ${token}`
      }
    });
  }

  getTourFreeSeats(tourId: number): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/getTourFreeSeats/${tourId}`);
  }

  hasUserPaidForTour(tourId: string, token: string): Observable<boolean> {
    return this.http.get<boolean>(`${this.apiUrl}/haveUserPayment/${tourId}`, {
      headers: {
        'Authorization': `Bearer ${token}`
      }
    });
  }

  reserveTour(payData: any, token: string) {
    return this.http.post(`${this.apiUrl}/reserveTour`, payData, {
      headers: {
        'Authorization': `Bearer ${token}`
      }
    });
  }
}
