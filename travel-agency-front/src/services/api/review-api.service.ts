import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environment/environment';
import { Review } from '../../models/review.model';

@Injectable({
  providedIn: 'root'
})
export class ReviewApiService {
  private apiUrl : string= `${environment.server}/review`;

  constructor(private http : HttpClient) { }

  getUserTourReview(tourId: string, token : string) : Observable<Review | null>{
    return this.http.get<Review | null>(`${this.apiUrl}/getUserReview/${tourId}`, {
      headers: {
        'Authorization': `Bearer ${token}`
      }
    });
  }

  getTourReviews(id: string): Observable<Review[]> {
    return this.http.get<Review[]>(`${this.apiUrl}/getTourReviews/${id}`);
  }
}
