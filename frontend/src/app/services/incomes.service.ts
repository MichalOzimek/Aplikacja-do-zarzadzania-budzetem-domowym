import { inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';
import { environment } from '../environments/environments';

export interface Income {
  id: number;
  source: string;
  amount: number;
  date: string;
  accountId: number;
  account?: { id: number; name: string } | null;
}

@Injectable({ providedIn: 'root' })
export class IncomesService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/incomes`;

  incomes = signal<Income[]>([]);

  loadAll() {
    return this.http
      .get<Income[]>(this.apiUrl)
      .pipe(tap((list) => this.incomes.set(list)));
  }

  getIncomes() {
    return this.http.get<Income[]>(this.apiUrl);
  }
  getIncome(id: number) {
    return this.http.get<Income>(`${this.apiUrl}/${id}`);
  }

  addIncome(payload: Partial<Income>) {
    return this.http
      .post<Income>(this.apiUrl, payload)
      .pipe(tap((created) => this.incomes.update((arr) => [created, ...arr])));
  }

  updateIncome(id: number, payload: Partial<Income>) {
    return this.http
      .put<Income>(`${this.apiUrl}/${id}`, payload)
      .pipe(
        tap((updated) =>
          this.incomes.update((arr) =>
            arr.map((i) => (i.id === updated.id ? updated : i))
          )
        )
      );
  }

  deleteIncome(id: number) {
    return this.http
      .delete<void>(`${this.apiUrl}/${id}`)
      .pipe(
        tap(() => this.incomes.update((arr) => arr.filter((i) => i.id !== id)))
      );
  }
}
