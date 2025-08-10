import { Component, signal } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { Menubar } from 'primeng/menubar';
import { FormsModule } from '@angular/forms';
import { InputSwitchModule } from 'primeng/inputswitch';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-menu',
  standalone: true,
  imports: [
    Menubar,
    FormsModule,
    InputSwitchModule,
    CommonModule,
    RouterModule,
  ],
  templateUrl: './menu.component.html',
  styleUrl: './menu.component.scss',
})
export class MenuComponent {
  protected darkMode = false;

  protected readonly items = signal<MenuItem[]>([
    {
      label: 'Strona główna',
      icon: 'pi pi-home',
      routerLink: ['/'],
    },
    {
      label: 'Transakcje',
      icon: 'pi pi-wallet',
      routerLink: ['/transakcje'],
    },
    {
      label: 'Zarządzaj',
      icon: 'pi pi-cog',
      items: [
        {
          label: 'Kategorie',
          icon: 'pi pi-tags',
          routerLink: ['/kategorie'],
        },
        {
          label: 'Sklepy',
          icon: 'pi pi-shopping-cart',
          routerLink: ['/sklepy'],
        },
        {
          label: 'Konta',
          icon: 'pi pi-credit-card',
          routerLink: ['/konta'],
        },
      ],
    },
  ]);

  protected toggleTheme() {
    const element = document.querySelector('html');
    if (element !== null) {
      element.classList.toggle('my-app-dark');
    }
  }
}
