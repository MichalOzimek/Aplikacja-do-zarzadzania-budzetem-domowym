import { Component, OnInit, signal } from '@angular/core';
import { ToolbarModule } from 'primeng/toolbar';
import { TableModule } from 'primeng/table';
import { Account, AccountsService } from '../../services/accounts.service';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-accounts',
  imports: [
    ToolbarModule,
    TableModule,
    ButtonModule,
    DialogModule,
    InputTextModule,
    FormsModule,
    CommonModule,
  ],
  standalone: true,
  templateUrl: './accounts.component.html',
  styleUrl: './accounts.component.scss',
})
export class AccountsComponent implements OnInit {
  accounts = signal<Account[]>([]);
  showDialog = signal(false);
  editingAccount = signal<Account | null>(null);
  visible: boolean = false;

  constructor(private accountsService: AccountsService) {}

  ngOnInit() {
    this.loadAccounts();
  }

  loadAccounts() {
    this.accountsService.getAccounts().subscribe((data) => {
      this.accounts.set(data);
    });
  }

  openCreateDialog() {
    this.editingAccount.set({ id: 0, name: '', balance: 0 });
    this.showDialog.set(true);
  }

  openEditDialog(account: Account) {
    this.editingAccount.set({ ...account });
    this.showDialog.set(true);
  }

  saveAccount() {
    const account = this.editingAccount();
    if (!account) return;

    if (account.id === 0) {
      this.accountsService.addAccount(account).subscribe((created) => {
        this.accounts.set([...this.accounts(), created]);
        this.showDialog.set(false);
      });
    } else {
      this.accountsService.updateAccount(account).subscribe((updated) => {
        const updatedList = this.accounts().map((a) =>
          a.id === updated.id ? updated : a
        );
        this.accounts.set(updatedList);
        this.showDialog.set(false);
        this.editingAccount.set(null);
      });
    }
  }

  deleteAccount(account: Account) {
    if (!account.id) return;
    this.accountsService.deleteAccount(account.id).subscribe(() => {
      this.accounts.set(this.accounts().filter((a) => a.id !== account.id));
    });
  }

  closeDialog() {
    this.showDialog.set(false);
    this.editingAccount.set(null);
  }

  onDialogVisibleChange(visible: boolean) {
    this.showDialog.set(visible);

    if (!visible) {
      this.editingAccount.set(null);
    }
  }
}
