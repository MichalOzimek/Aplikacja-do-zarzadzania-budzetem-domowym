import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../environments/environments';

export interface Summary {
  incomes: number;
  expenses: number;
  balance: number;
}

export interface BalancePoint {
  date: string;
  incomes: number;
  expenses: number;
  balance: number;
}

@Injectable({
  providedIn: 'root',
})
export class SummaryService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/Summary`;

  getSummary(): Observable<Summary> {
    return this.http.get<Summary>(this.apiUrl);
  }

  getMonthlySummary(month: number, year: number): Observable<Summary> {
    return this.http.get<Summary>(`${this.apiUrl}/month/${month}/year/${year}`);
  }

  getBalanceHistory(): Observable<BalancePoint[]> {
    return this.http.get<BalancePoint[]>(`${this.apiUrl}/balance-history`);
  }
}
