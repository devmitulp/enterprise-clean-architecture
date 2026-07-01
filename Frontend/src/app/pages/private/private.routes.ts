import { Routes } from '@angular/router';
import { ROUTE_PATHS } from '@constants';
import { PrivateLayoutComponent } from '@layout';
import { authGuard } from '@auth';
import { healthGuard } from '@guards';

export const PRIVATE_ROUTES: Routes = [
  {
    path: '',
    component: PrivateLayoutComponent,
    canActivate: [healthGuard, authGuard],
    children: [
      {
        path: ROUTE_PATHS.DASHBOARD,
        loadComponent: () =>
          import('./dashboard/dashboard.component').then((m) => m.DashboardComponent),
      },
      {
        path: ROUTE_PATHS.SETTINGS,
        loadComponent: () =>
          import('./settings/settings.component').then((m) => m.SettingsComponent),
      },
    ],
  },
];
