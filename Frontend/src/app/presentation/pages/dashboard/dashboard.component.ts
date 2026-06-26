import { Component, ChangeDetectionStrategy, signal } from '@angular/core';

interface DashboardStats {
  label: string;
  value: string;
  trend: string;
  trendUp: boolean;
  icon: string;
}

@Component({
  selector: 'app-dashboard',
  standalone: true,
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DashboardComponent {
  readonly stats = signal<DashboardStats[]>([
    { label: 'Active Users', value: '1,482', trend: '+12.4%', trendUp: true, icon: 'pi pi-users' },
    { label: 'Total Revenue', value: '$45,231', trend: '+8.2%', trendUp: true, icon: 'pi pi-dollar' },
    { label: 'Pending Approvals', value: '18', trend: '-2.4%', trendUp: false, icon: 'pi pi-file-edit' },
    { label: 'System Health', value: '99.9%', trend: '+0.01%', trendUp: true, icon: 'pi pi-check-circle' }
  ]);
}
