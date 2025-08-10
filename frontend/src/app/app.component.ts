import { Component, OnInit, inject } from '@angular/core';
import { PrimeNG } from 'primeng/config';
import { MenuComponent } from './components/menu/menu.component';
import { RouterOutlet } from '@angular/router';
import { DialogModule } from 'primeng/dialog';
import { IncomesService } from './services/incomes.service';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { AccountsService, Account } from './services/accounts.service';
import { DropdownModule } from 'primeng/dropdown';
import { PurchaseHttpService } from './services/purchase.service';
import { CategoryHttpService } from './services/category.service';
import { ShopsHttpService } from './services/shops.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    MenuComponent,
    RouterOutlet,
    DialogModule,
    FormsModule,
    ButtonModule,
    DropdownModule,
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent implements OnInit {
  incomeService = inject(IncomesService);
  purchuaseService = inject(PurchaseHttpService);
  accountsService = inject(AccountsService);
  categoryService = inject(CategoryHttpService);
  shopService = inject(ShopsHttpService);
  purchaseService = inject(PurchaseHttpService);
  incomesService = inject(IncomesService);

  showIncomeDialog = false;
  showExpenseDialog = false;

  accounts: Account[] = [];
  categories: any[] = [];
  shops: any[] = [];

  transactions: {
    id: number;
    sourceType: 'income' | 'purchase';
    date: string;
    type: 'Wpływ' | 'Wydatek';
    description: string;
    amount: number;
    accountName: string;
  }[] = [];

  newIncome = {
    source: '',
    amount: 0,
    date: new Date().toISOString(),
    accountId: 1,
  };

  newPurchuase = {
    billCost: 0,
    date: new Date().toISOString(),
    note: '',
    categoryId: null,
    shopId: null,
    accountId: 0,
  };

  constructor(private primeng: PrimeNG) {}

  ngOnInit() {
    this.primeng.ripple.set(true);

    this.accountsService.getAccounts().subscribe((data) => {
      this.accounts = data;
      if (data.length > 0) {
        this.newIncome.accountId = data[0].id;
      }
    });

    this.categoryService.getAll().subscribe((data) => {
      this.categories = data;
    });

    this.shopService.getAll().subscribe((data) => {
      this.shops = data;
    });
  }

  submitIncome() {
    this.incomeService.addIncome(this.newIncome).subscribe({
      next: () => {
        this.showIncomeDialog = false;

        this.newIncome = {
          source: '',
          amount: 0,
          date: new Date().toISOString(),
          accountId: 1,
        };
      },
      error: (err) => {
        console.error('Błąd przy dodawaniu wpływu:', err);
      },
    });
  }

  submitExpense() {
    this.purchuaseService.create(this.newPurchuase).subscribe({
      next: () => {
        this.showExpenseDialog = false;

        this.newPurchuase = {
          billCost: 0,
          date: new Date().toISOString(),
          note: '',
          categoryId: null,
          shopId: null,
          accountId: this.accounts[0]?.id ?? 1,
        };
        this.loadTransactions();
      },
      error: (err) => {
        console.error('Błąd przy dodawaniu wydatku:', err);
      },
    });
  }

  loadTransactions() {
    this.incomesService.getIncomes().subscribe((incomes) => {
      const mappedIncomes = incomes.map((income: any) => ({
        id: income.id,
        date: income.date as string,
        type: 'Wpływ' as const,
        description: income.source as string,
        amount: income.amount as number,
        accountName: income.account?.name ?? 'Brak',
        sourceType: 'income' as const,
      }));

      this.purchaseService.getAll().subscribe((purchases) => {
        const mappedExpenses = purchases.map((p: any) => ({
          id: p.id,
          date: p.date as string,
          type: 'Wydatek' as const,
          description: p.note as string,
          amount: p.billCost as number,
          accountName: p.account?.name ?? 'Brak',
          sourceType: 'purchase' as const,
        }));

        this.transactions = [...mappedIncomes, ...mappedExpenses].sort(
          (a, b) => new Date(b.date).getTime() - new Date(a.date).getTime()
        );
      });
    });
  }
}
