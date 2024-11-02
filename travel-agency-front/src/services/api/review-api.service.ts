import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environment/environment';
import { Review } from '../../models/review.model';

@Injectable({
  providedIn: 'root'
})
export class ReviewApiService {
  private apiUrl: string = `${environment.server}/review`;

  constructor(private http: HttpClient) { }

  getUserTourReview(tourId: number, token: string): Observable<Review | null> {
    return this.http.get<Review | null>(`${this.apiUrl}/getUserReview/${tourId}`, {
      headers: {
        'Authorization': `Bearer ${token}`
      }
    });
  }

  getTourReviews(id: number): Observable<Review[]> {
    return this.http.get<Review[]>(`${this.apiUrl}/getTourReviews/${id}`);
  }

  createReview(review: Review, token: string) {
    return this.http.post(`${this.apiUrl}/create`, review, {
      headers: {
        'Authorization': `Bearer ${token}`
      }
    });
  }

  updateReview(review: Review, token: string) {
    return this.http.patch(`${this.apiUrl}/update`, review, {
      headers: {
        'Authorization': `Bearer ${token}`
      }
    });
  }

  deleteReview(id: number, token: string) {
    return this.http.delete(`${this.apiUrl}/delete/${id}`, {
      headers: {
        'Authorization': `Bearer ${token}`
      }
    });
  }
}
