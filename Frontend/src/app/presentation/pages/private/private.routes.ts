import { Routes } from '@angular/router';
import { ROUTE_PATHS } from '../../../core/constants/routes.constants';
import { PrivateLayoutComponent } from '../../layouts/private-layout/private-layout.component';
import { authGuard } from '../../../core/guards/auth.guard';
import { healthGuard } from '../../../core/guards/health.guard';

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
