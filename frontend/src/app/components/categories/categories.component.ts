import { Component, inject, OnInit, signal } from '@angular/core';
import { ToolbarModule } from 'primeng/toolbar';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CategoryHttpService } from '../../services/category.service';

export interface Category {
  id: number;
  name: string;
}

@Component({
  selector: 'app-categories',
  standalone: true,
  templateUrl: './categories.component.html',
  styleUrl: './categories.component.scss',
  imports: [
    ToolbarModule,
    TableModule,
    ButtonModule,
    DialogModule,
    InputTextModule,
    FormsModule,
    CommonModule,
  ],
})
export class CategoriesComponent implements OnInit {
  categoryService = inject(CategoryHttpService);

  categories = signal<Category[]>([]);
  showDialog = signal(false);
  editingCategory = signal<Category | null>(null);

  ngOnInit(): void {
    this.loadCategories();
  }

  loadCategories() {
    this.categoryService.getAll().subscribe((data) => {
      this.categories.set(data);
    });
  }

  openCreateDialog() {
    this.editingCategory.set({ id: 0, name: '' });
    this.showDialog.set(true);
  }

  openEditDialog(cat: Category) {
    this.editingCategory.set({ ...cat });
    this.showDialog.set(true);
  }

  saveCategory() {
    const category = this.editingCategory();
    if (!category) return;

    if (category.id === 0) {
      this.categoryService.create(category).subscribe((created) => {
        this.categories.set([...this.categories(), created]);
        this.showDialog.set(false);
      });
    } else {
      this.categoryService
        .update(category.id, category)
        .subscribe((updated) => {
          const updatedList = this.categories().map((c) =>
            c.id === updated.id ? updated : c
          );
          this.categories.set(updatedList);
          this.showDialog.set(false);
          this.editingCategory.set(null);
        });
    }
  }

  deleteCategory(cat: Category) {
    this.categoryService.delete(cat.id).subscribe(() => {
      this.categories.set(this.categories().filter((c) => c.id !== cat.id));
    });
  }

  closeDialog() {
    this.showDialog.set(false);
    this.editingCategory.set(null);
  }

  onDialogVisibleChange(visible: boolean) {
    this.showDialog.set(visible);
    if (!visible) this.editingCategory.set(null);
  }
}
