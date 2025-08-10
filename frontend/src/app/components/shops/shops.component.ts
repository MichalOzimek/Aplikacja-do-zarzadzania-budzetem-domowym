import { Component, inject, OnInit, signal } from '@angular/core';
import { ToolbarModule } from 'primeng/toolbar';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ShopsHttpService } from '../../services/shops.service';

export interface Shop {
  id: number;
  name: string;
}

@Component({
  selector: 'app-shops',
  standalone: true,
  imports: [
    ToolbarModule,
    TableModule,
    ButtonModule,
    DialogModule,
    InputTextModule,
    FormsModule,
    CommonModule,
  ],
  templateUrl: './shops.component.html',
  styleUrl: './shops.component.scss',
})
export class ShopsComponent implements OnInit {
  shopService = inject(ShopsHttpService);

  shops = signal<Shop[]>([]);
  showDialog = signal(false);
  editingShop = signal<Shop | null>(null);

  ngOnInit() {
    this.loadShops();
  }

  loadShops() {
    this.shopService.getAll().subscribe((data) => this.shops.set(data));
  }

  openCreateDialog() {
    this.editingShop.set({ id: 0, name: '' });
    this.showDialog.set(true);
  }

  openEditDialog(shop: Shop) {
    this.editingShop.set({ ...shop });
    this.showDialog.set(true);
  }

  saveShop() {
    const shop = this.editingShop();
    if (!shop) return;

    if (shop.id === 0) {
      this.shopService.create(shop).subscribe((created) => {
        this.shops.set([...this.shops(), created]);
        this.showDialog.set(false);
      });
    } else {
      this.shopService.update(shop.id, shop).subscribe((updated) => {
        const updatedList = this.shops().map((s) =>
          s.id === updated.id ? updated : s
        );
        this.shops.set(updatedList);
        this.showDialog.set(false);
        this.editingShop.set(null);
      });
    }
  }

  deleteShop(shop: Shop) {
    if (!shop.id) return;
    this.shopService.delete(shop.id).subscribe(() => {
      this.shops.set(this.shops().filter((s) => s.id !== shop.id));
    });
  }

  closeDialog() {
    this.showDialog.set(false);
    this.editingShop.set(null);
  }

  onDialogVisibleChange(visible: boolean) {
    this.showDialog.set(visible);
    if (!visible) this.editingShop.set(null);
  }
}
