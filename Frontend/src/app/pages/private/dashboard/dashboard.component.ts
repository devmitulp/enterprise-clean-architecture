import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { TranslatePipe } from '@ngx-translate/core';
import { AuthState } from '@auth';

interface DashboardStats {
  LabelKey: string;
  Value?: string;
  ValueKey?: string;
  Trend: string;
  TrendUp: boolean;
  Icon: string;
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
      LabelKey: 'ActiveUsers',
      Value: '1,482',
      Trend: '+12.4%',
      TrendUp: true,
      Icon: 'fa fa-users',
    },
    {
      LabelKey: 'TotalRevenue',
      Value: '$45,231',
      Trend: '+8.2%',
      TrendUp: true,
      Icon: 'fa-solid fa-dollar-sign',
    },
    {
      LabelKey: 'PendingApprovals',
      Value: '18',
      Trend: '-2.4%',
      TrendUp: false,
      Icon: 'fa fa-file-edit',
    },
    {
      LabelKey: 'SystemHealth',
      Value: '99.9%',
      Trend: '+0.01%',
      TrendUp: true,
      Icon: 'fa-regular fa-circle-check',
    },
  ];
}
