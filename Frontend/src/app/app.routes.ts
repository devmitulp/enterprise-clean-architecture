import { Routes } from '@angular/router';
import { ROUTE_PATHS } from './core/constants/routes.constants';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: ROUTE_PATHS.LOGIN
  },
  // Public Section (Uses PublicLayoutComponent as Shell)
  {
    path: '',
    loadComponent: () => import('./presentation/layouts/public-layout/public-layout.component')
      .then(m => m.PublicLayoutComponent),
    children: [
      {
        path: ROUTE_PATHS.LOGIN,
        loadComponent: () => import('./presentation/pages/login/login.component')
          .then(m => m.LoginComponent)
      }
    ]
  },

  // Wildcard Fallback Route to Login Page
  {
    path: '**',
    redirectTo: ROUTE_PATHS.LOGIN,
    pathMatch: 'full'
  }
];
