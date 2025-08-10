import {
  Component,
  OnInit,
  inject,
  signal,
  computed,
  effect,
  Injector,
} from '@angular/core';
import { CardModule } from 'primeng/card';
import { ChartModule } from 'primeng/chart';
import { AccountsService, Account } from '../../services/accounts.service';
import { CommonModule } from '@angular/common';
import { CurrencyPipe } from '../shared/currency.pipe';
import { Summary, SummaryService } from '../../services/summary.service';
import { IncomesService } from '../../services/incomes.service';
import { PurchaseHttpService } from '../../services/purchase.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CardModule, ChartModule, CommonModule, CurrencyPipe],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
})
export class DashboardComponent implements OnInit {
  private accountsService = inject(AccountsService);
  private summaryService = inject(SummaryService);
  private incomesService = inject(IncomesService);
  private purchaseService = inject(PurchaseHttpService);

  accounts: Account[] = [];

  summary = signal<Summary | null>(null);
  summaryPrevious = signal<Summary | null>(null);
  previousMonthLabel = '';

  chartData: any;
  chartOptions: any;
  chartDataPrevious: any;
  chartOptionsPrevious: any;
  balanceChartData: any;
  balanceChartOptions: any;

  recentTransactions = computed(() => {
    const incomes = this.incomesService.incomes?.() ?? [];
    const purchases = this.purchaseService.purchases?.() ?? [];

    const incomeTxs = incomes.map((i: any) => ({
      date: i.date,
      type: 'Wpływ' as const,
      description: i.source,
      amount: i.amount,
      accountName: i.account?.name ?? 'Brak',
    }));

    const expenseTxs = purchases.map((p: any) => ({
      date: p.date,
      type: 'Wydatek' as const,
      description: p.note,
      amount: p.billCost,
      accountName: p.account?.name ?? 'Brak',
    }));

    return [...incomeTxs, ...expenseTxs]
      .sort((a, b) => new Date(b.date).getTime() - new Date(a.date).getTime())
      .slice(0, 10);
  });

  ngOnInit() {
    this.incomesService.loadAll?.().subscribe();
    this.purchaseService.loadAll?.().subscribe();

    this.accountsService.getAccounts().subscribe((d) => (this.accounts = d));

    this.chartOptions = {
      responsive: true,
      plugins: { legend: { display: false } },
      scales: { x: { display: false }, y: { display: false } },
    };
    this.chartOptionsPrevious = {
      responsive: true,
      plugins: { legend: { display: false } },
      scales: { x: { display: false }, y: { display: false } },
    };

    const today = new Date();
    const prev = new Date(today.getFullYear(), today.getMonth() - 1, 1);
    const monthNames = [
      'Styczeń',
      'Luty',
      'Marzec',
      'Kwiecień',
      'Maj',
      'Czerwiec',
      'Lipiec',
      'Sierpień',
      'Wrzesień',
      'Październik',
      'Listopad',
      'Grudzień',
    ];
    this.previousMonthLabel = `${
      monthNames[prev.getMonth()]
    } ${prev.getFullYear()}`;
  }

  private readonly refreshEffect = effect(
    () => {
      this.incomesService.incomes?.();
      this.purchaseService.purchases?.();

      this.reloadSummariesAndCharts();

      this.accountsService.getAccounts().subscribe((d) => (this.accounts = d));
    },
    { injector: inject(Injector) }
  );

  private reloadSummariesAndCharts() {
    this.summaryService.getSummary().subscribe((data) => {
      this.summary.set(data);
      this.chartData = {
        labels: ['Wpływy', 'Wydatki'],
        datasets: [
          {
            label: 'Ten miesiąc',
            data: [data.incomes, data.expenses],
            backgroundColor: ['#4CAF50', '#F44336'],
          },
        ],
      };
    });

    const now = new Date();
    const prev = new Date(now.getFullYear(), now.getMonth() - 1, 1);
    const prevMonth = prev.getMonth() + 1;
    const prevYear = prev.getFullYear();

    this.summaryService
      .getMonthlySummary(prevMonth, prevYear)
      .subscribe((prevSum) => {
        this.summaryPrevious.set(prevSum);
        this.chartDataPrevious =
          prevSum.incomes === 0 && prevSum.expenses === 0
            ? {
                labels: ['Brak danych'],
                datasets: [{ data: [1], backgroundColor: ['#ccc'] }],
              }
            : {
                labels: ['Wpływy', 'Wydatki'],
                datasets: [
                  {
                    data: [prevSum.incomes, prevSum.expenses],
                    backgroundColor: ['#81C784', '#E57373'],
                  },
                ],
              };
      });

    this.summaryService.getBalanceHistory().subscribe((points) => {
      const labels = points.map((pt) =>
        new Date(pt.date).toLocaleDateString('pl-PL', { weekday: 'short' })
      );
      const incomes = points.map((pt) => Math.abs(pt.incomes));
      const expenses = points.map((pt) => Math.abs(pt.expenses));

      this.balanceChartData = {
        labels,
        datasets: [
          {
            label: 'Wpływy',
            data: incomes,
            borderColor: '#4CAF50',
            backgroundColor: 'rgba(76, 175, 80, 0.2)',
            fill: false,
            tension: 0.3,
          },
          {
            label: 'Wydatki',
            data: expenses,
            borderColor: '#F44336',
            backgroundColor: 'rgba(244, 67, 54, 0.2)',
            fill: false,
            tension: 0.3,
          },
        ],
      };
      this.balanceChartOptions = {
        responsive: true,
        maintainAspectRatio: false,
        plugins: { legend: { display: true } },
        scales: { y: { beginAtZero: true, min: 0 } },
      };
    });
  }
}
