import { Routes } from '@shared/angular';
import { ROUTE_PATHS } from '@constants';
import { maintenanceGuard } from '@guards';

export const ERROR_ROUTES: Routes = [
  // Error Pages (Directly loaded, no layout wrap required)
  {
    path: ROUTE_PATHS.ACCESS_DENIED,
    loadComponent: () =>
      import('./unauthorized/unauthorized.component').then((m) => m.UnauthorizedComponent),
  },
  {
    path: ROUTE_PATHS.SERVER_ERROR,
    loadComponent: () =>
      import('./server-error/server-error.component').then((m) => m.ServerErrorComponent),
  },
  {
    path: ROUTE_PATHS.NOT_FOUND,
    loadComponent: () => import('./not-found/not-found.component').then((m) => m.NotFoundComponent),
  },
  {
    path: ROUTE_PATHS.MAINTENANCE,
    loadComponent: () =>
      import('./maintenance/maintenance.component').then((m) => m.MaintenanceComponent),
    canActivate: [maintenanceGuard],
  },
];
