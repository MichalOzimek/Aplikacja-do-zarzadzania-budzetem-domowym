import { Component, inject, OnInit, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TableModule } from 'primeng/table';
import { ToolbarModule } from 'primeng/toolbar';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { FormsModule } from '@angular/forms';
import { DropdownModule } from 'primeng/dropdown';

import { Income, IncomesService } from '../../services/incomes.service';
import { PurchaseHttpService } from '../../services/purchase.service';
import { AccountsService, Account } from '../../services/accounts.service';
import { CategoryHttpService } from '../../services/category.service';

@Component({
  selector: 'app-transactions',
  standalone: true,
  imports: [
    CommonModule,
    TableModule,
    ToolbarModule,
    ButtonModule,
    DialogModule,
    FormsModule,
    DropdownModule,
  ],
  templateUrl: './transactions.component.html',
  styleUrl: './transactions.component.scss',
})
export class TransactionsComponent implements OnInit {
  incomesService = inject(IncomesService);
  purchaseService = inject(PurchaseHttpService);
  accountsService = inject(AccountsService);
  categoryService = inject(CategoryHttpService);

  accounts: Account[] = [];
  categories: any[] = [];

  editingIncome: Income | null = null;
  editingPurchase: any = null;
  showEditDialog = false;

  transactions = computed(() => {
    const mappedIncomes = this.incomesService.incomes().map((income) => ({
      id: income.id,
      date: income.date,
      type: 'Wpływ' as const,
      description: income.source,
      amount: income.amount,
      accountName: income.account?.name ?? 'Brak',
      sourceType: 'income' as const,
    }));

    const mappedExpenses = this.purchaseService.purchases().map((p: any) => ({
      id: p.id,
      date: p.date as string,
      type: 'Wydatek' as const,
      description: p.note as string,
      amount: p.billCost as number,
      accountName: p.account?.name ?? 'Brak',
      sourceType: 'purchase' as const,
    }));

    return [...mappedIncomes, ...mappedExpenses].sort(
      (a, b) => new Date(b.date).getTime() - new Date(a.date).getTime()
    );
  });

  ngOnInit() {
    this.purchaseService.loadAll().subscribe();
    this.incomesService.loadAll().subscribe();

    this.loadAccounts();
    this.loadCategories();
  }

  loadCategories() {
    this.categoryService.getAll().subscribe((data) => (this.categories = data));
  }

  loadAccounts() {
    this.accountsService
      .getAccounts()
      .subscribe((data) => (this.accounts = data));
  }

  editTransaction(tx: any) {
    if (tx.sourceType === 'income') {
      this.incomesService.getIncome(tx.id).subscribe((income) => {
        this.openEditIncomeDialog(income);
      });
    } else {
      this.purchaseService.getById(tx.id).subscribe((purchase) => {
        this.openEditPurchaseDialog(purchase);
      });
    }
  }

  deleteTransaction(tx: any) {
    if (tx.sourceType === 'income') {
      this.incomesService.deleteIncome(tx.id).subscribe();
    } else {
      this.purchaseService.delete(tx.id).subscribe();
    }
  }

  openEditIncomeDialog(income: Income) {
    this.editingIncome = { ...income };
    this.showEditDialog = true;
  }

  openEditPurchaseDialog(purchase: any) {
    this.editingPurchase = { ...purchase };
    this.showEditDialog = true;
  }

  saveIncome() {
    if (!this.editingIncome) return;

    const payload = {
      id: this.editingIncome.id,
      source: this.editingIncome.source,
      amount: this.editingIncome.amount,
      date: new Date(this.editingIncome.date).toISOString(),
      accountId: this.editingIncome.accountId,
    };

    this.incomesService.updateIncome(this.editingIncome.id, payload).subscribe({
      next: () => this.closeEditDialog(),
    });
  }

  savePurchase() {
    if (!this.editingPurchase) return;

    const payload = {
      id: this.editingPurchase.id,
      note: this.editingPurchase.note,
      billCost: this.editingPurchase.billCost,
      date: new Date(this.editingPurchase.date).toISOString(),
      accountId: this.editingPurchase.accountId,
      categoryId: this.editingPurchase.categoryId ?? null,
      shopId: this.editingPurchase.shopId ?? null,
    };

    this.purchaseService.update(this.editingPurchase.id, payload).subscribe({
      next: () => this.closeEditDialog(),
      error: (err) =>
        console.error('Błąd przy aktualizacji wydatku:', err?.error ?? err),
    });
  }

  closeEditDialog() {
    this.editingIncome = null;
    this.editingPurchase = null;
    this.showEditDialog = false;
  }
}
