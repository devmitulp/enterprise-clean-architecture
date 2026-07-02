import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { TranslatePipe } from '@ngx-translate/core';
import { AuthState } from '@auth';

interface DashboardStats {
  labelKey: string;
  value?: string;
  valueKey?: string;
  trend: string;
  trendUp: boolean;
  icon: string;
}

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [TranslatePipe],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DashboardComponent {
  private readonly authState = inject(AuthState);

  readonly userName = this.authState.userName;

  readonly stats: DashboardStats[] = [
    {
      labelKey: 'ActiveUsers',
      value: '1,482',
      trend: '+12.4%',
      trendUp: true,
      icon: 'fa fa-users',
    },
    {
      labelKey: 'TotalRevenue',
      valueKey: 'RevenueValue',
      trend: '+8.2%',
      trendUp: true,
      icon: 'fa-solid fa-dollar-sign',
    },
    {
      labelKey: 'PendingApprovals',
      value: '18',
      trend: '-2.4%',
      trendUp: false,
      icon: 'fa fa-file-edit',
    },
    {
      labelKey: 'SystemHealth',
      value: '99.9%',
      trend: '+0.01%',
      trendUp: true,
      icon: 'fa-regular fa-circle-check',
    },
  ];
}
