import { Routes } from '@angular/router';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { TransactionsComponent } from './components/transactions/transactions.component';
import { AccountsComponent } from './components/accounts/accounts.component';
import { CategoriesComponent } from './components/categories/categories.component';
import { ShopsComponent } from './components/shops/shops.component';

export const routes: Routes = [
  {
    path: '',
    component: DashboardComponent,
  },
  {
    path: 'transakcje',
    component: TransactionsComponent,
  },
  {
    path: 'konta',
    component: AccountsComponent,
  },
  {
    path: 'kategorie',
    component: CategoriesComponent,
  },
  {
    path: 'sklepy',
    component: ShopsComponent,
  },
];
