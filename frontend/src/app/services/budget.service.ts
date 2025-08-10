import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environments';

@Injectable({ providedIn: 'root' })
export class BudgetHttpService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/MonthlyBudget`;

  getMonthlyCategoriesBudget(month: number, year: number): Observable<any[]> {
    return this.http.get<any[]>(
      `${this.apiUrl}/monthlyCategoriesBudget/${month}/${year}`
    );
  }

  getMonthlyShopsBudget(month: number, year: number): Observable<any[]> {
    return this.http.get<any[]>(
      `${this.apiUrl}/monthlyShopsBudget/${month}/${year}`
    );
  }
}
