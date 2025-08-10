import { inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';
import { environment } from '../environments/environments';

@Injectable({ providedIn: 'root' })
export class PurchaseHttpService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/purchases`;

  purchases = signal<any[]>([]);

  loadAll() {
    return this.http
      .get<any[]>(this.apiUrl)
      .pipe(tap((list) => this.purchases.set(list)));
  }

  getAll() {
    return this.http.get<any[]>(this.apiUrl);
  }
  getById(id: number) {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  create(purchase: any) {
    return this.http
      .post<any>(this.apiUrl, purchase)
      .pipe(
        tap((created) => this.purchases.update((arr) => [created, ...arr]))
      );
  }

  update(id: number, purchase: any) {
    return this.http
      .put<any>(`${this.apiUrl}/${id}`, purchase)
      .pipe(
        tap((updated) =>
          this.purchases.update((arr) =>
            arr.map((p) => (p.id === updated.id ? updated : p))
          )
        )
      );
  }

  delete(id: number) {
    return this.http
      .delete<void>(`${this.apiUrl}/${id}`)
      .pipe(
        tap(() =>
          this.purchases.update((arr) => arr.filter((p) => p.id !== id))
        )
      );
  }
}
