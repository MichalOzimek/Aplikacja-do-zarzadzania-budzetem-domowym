import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environments';

export interface Account {
  id: number;
  name: string;
  balance: number;
}

@Injectable({ providedIn: 'root' })
export class AccountsService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/Accounts`;

  getAccounts(): Observable<Account[]> {
    return this.http.get<Account[]>(this.apiUrl);
  }

  getAccount(id: number): Observable<Account> {
    return this.http.get<Account>(`${this.apiUrl}/${id}`);
  }

  addAccount(account: Omit<Account, 'id'>): Observable<Account> {
    return this.http.post<Account>(this.apiUrl, account);
  }

  updateAccount(account: Account): Observable<Account> {
    return this.http.put<Account>(`${this.apiUrl}/${account.id}`, account);
  }

  deleteAccount(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
