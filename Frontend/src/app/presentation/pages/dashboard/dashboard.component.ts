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
  template: `
    <div class="space-y-6">
      
      <!-- Welcome Header -->
      <div>
        <h1 class="text-2xl font-bold tracking-tight text-slate-900 dark:text-white">Workspace Overview</h1>
        <p class="text-sm text-slate-500 dark:text-slate-400">Welcome back, John! Here is what's happening today.</p>
      </div>

      <!-- Stats Grid -->
      <div class="grid grid-cols-1 gap-5 sm:grid-cols-2 lg:grid-cols-4">
        @for (stat of stats(); track stat.label) {
          <div class="relative overflow-hidden rounded-2xl bg-white dark:bg-slate-900 border border-slate-200 dark:border-slate-800 p-6 shadow-sm hover:shadow-md transition-all duration-200">
            <div class="flex items-center justify-between">
              <div>
                <p class="text-xs font-semibold text-slate-500 dark:text-slate-400 uppercase tracking-wider">{{ stat.label }}</p>
                <h3 class="mt-2 text-3xl font-bold leading-none">{{ stat.value }}</h3>
              </div>
              <div class="flex h-12 w-12 items-center justify-center rounded-xl bg-violet-50 text-violet-600 dark:bg-violet-950/40 dark:text-violet-400">
                <i [class]="stat.icon + ' text-xl'"></i>
              </div>
            </div>
            <div class="mt-4 flex items-center gap-1.5 text-xs">
              <span [class]="stat.trendUp ? 'text-emerald-600 dark:text-emerald-400' : 'text-rose-600 dark:text-rose-400'" class="font-bold flex items-center">
                <i [class]="stat.trendUp ? 'pi pi-arrow-up-right' : 'pi pi-arrow-down-left'" class="text-[10px] mr-0.5"></i>
                {{ stat.trend }}
              </span>
              <span class="text-slate-500">vs last month</span>
            </div>
          </div>
        }
      </div>

      <!-- Quick Actions / Main section placeholder -->
      <div class="grid grid-cols-1 gap-6 lg:grid-cols-3">
        <div class="lg:col-span-2 rounded-2xl border border-slate-200 dark:border-slate-800 bg-white dark:bg-slate-900 p-6 h-96 flex flex-col justify-center items-center text-center">
          <i class="pi pi-chart-line text-4xl text-slate-300 dark:text-slate-700 mb-4"></i>
          <h3 class="text-lg font-semibold text-slate-900 dark:text-white">Performance Analytics</h3>
          <p class="text-sm text-slate-500 max-w-sm mt-1">Analytics charts will display here once backend service integration is completed.</p>
        </div>
        <div class="rounded-2xl border border-slate-200 dark:border-slate-800 bg-white dark:bg-slate-900 p-6 h-96 flex flex-col justify-center items-center text-center">
          <i class="pi pi-list text-4xl text-slate-300 dark:text-slate-700 mb-4"></i>
          <h3 class="text-lg font-semibold text-slate-900 dark:text-white">Recent Activities</h3>
          <p class="text-sm text-slate-500 max-w-xs mt-1">A timeline of system events, audit logs, and status updates will be displayed here.</p>
        </div>
      </div>

    </div>
  `,
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
