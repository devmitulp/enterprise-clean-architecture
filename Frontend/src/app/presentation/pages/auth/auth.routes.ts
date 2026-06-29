import { Routes } from '@angular/router';
import { ROUTE_PATHS } from '../../../core/constants/routes.constants';
import { BlankLayoutComponent } from '../../layouts/blank-layout/blank-layout.component';
import { publicGuard } from '../../../core/guards/public.guard';

export const AUTH_ROUTES: Routes = [
  {
    path: '',
    component: BlankLayoutComponent,
    canActivate: [publicGuard],
    children: [
      {
        path: ROUTE_PATHS.LOGIN,
        loadComponent: () => import('./login/login.component').then((m) => m.LoginComponent),
      },
      {
        path: ROUTE_PATHS.FORGOT_PASSWORD,
        loadComponent: () =>
          import('./forgot-password/forgot-password.component').then(
            (m) => m.ForgotPasswordComponent,
          ),
      },
      {
        path: ROUTE_PATHS.RESET_PASSWORD,
        loadComponent: () =>
          import('./reset-password/reset-password.component').then((m) => m.ResetPasswordComponent),
      },
      {
        path: ROUTE_PATHS.MFA,
        loadComponent: () => import('./mfa/mfa.component').then((m) => m.MfaComponent),
      },
    ],
  },
];
