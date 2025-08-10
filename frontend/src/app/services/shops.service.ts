import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environments';

@Injectable({ providedIn: 'root' })
export class ShopsHttpService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/shops`;

  getAll(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  getById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  create(shop: any): Observable<any> {
    return this.http.post<any>(this.apiUrl, shop);
  }

  update(id: number, shop: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${id}`, shop);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
