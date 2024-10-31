import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environment/environment';
import { Transport } from '../../models/transport.model';

@Injectable({
  providedIn: 'root'
})
export class TransportApiService {
  private apiUrl : string = `${environment.server}/transport`;

  constructor(private http : HttpClient) { }

  getTransports() : Observable<Transport[]> {
    return this.http.get<Transport[]>(`${this.apiUrl}/getAllTransports`);
  }
}
