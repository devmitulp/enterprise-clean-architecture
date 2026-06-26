import { Routes } from '@angular/router';
import { ROUTE_PATHS } from './core/constants/routes.constants';

export const routes: Routes = [
  // Public Section (Uses PublicLayoutComponent as Shell)
  {
    path: '',
    loadComponent: () => import('./presentation/layouts/public-layout/public-layout.component')
      .then(m => m.PublicLayoutComponent),
    children: [
      {
        path: ROUTE_PATHS.HOME,
        loadComponent: () => import('./presentation/pages/home/home.component')
          .then(m => m.HomeComponent)
      },
      {
        path: ROUTE_PATHS.ABOUT,
        loadComponent: () => import('./presentation/pages/about/about.component')
          .then(m => m.AboutComponent)
      },
      {
        path: ROUTE_PATHS.CONTACT,
        loadComponent: () => import('./presentation/pages/contact/contact.component')
          .then(m => m.ContactComponent)
      },
      {
        path: ROUTE_PATHS.LOGIN,
        loadComponent: () => import('./presentation/pages/login/login.component')
          .then(m => m.LoginComponent)
      },
      {
        path: ROUTE_PATHS.FORGOT_PASSWORD,
        loadComponent: () => import('./presentation/pages/forgot-password/forgot-password.component')
          .then(m => m.ForgotPasswordComponent)
      },
      {
        path: ROUTE_PATHS.RESET_PASSWORD,
        loadComponent: () => import('./presentation/pages/reset-password/reset-password.component')
          .then(m => m.ResetPasswordComponent)
      },
      {
        path: ROUTE_PATHS.PRIVACY,
        loadComponent: () => import('./presentation/pages/privacy/privacy.component')
          .then(m => m.PrivacyComponent)
      },
      {
        path: ROUTE_PATHS.TERMS,
        loadComponent: () => import('./presentation/pages/terms/terms.component')
          .then(m => m.TermsComponent)
      }
    ]
  },

  // Private Section (Uses PrivateLayoutComponent as Shell)
  {
    path: '',
    loadComponent: () => import('./presentation/layouts/private-layout/private-layout.component')
      .then(m => m.PrivateLayoutComponent),
    children: [
      {
        path: ROUTE_PATHS.DASHBOARD,
        loadComponent: () => import('./presentation/pages/dashboard/dashboard.component')
          .then(m => m.DashboardComponent)
      },
      {
        path: ROUTE_PATHS.SETTINGS,
        loadComponent: () => import('./presentation/pages/settings/settings.component')
          .then(m => m.SettingsComponent)
      }
    ]
  },

  // Error Pages (Directly loaded, no layout wrap required)
  {
    path: ROUTE_PATHS.UNAUTHORIZED,
    loadComponent: () => import('./presentation/pages/errors/unauthorized/unauthorized.component')
      .then(m => m.UnauthorizedComponent)
  },
  {
    path: ROUTE_PATHS.SERVER_ERROR,
    loadComponent: () => import('./presentation/pages/errors/server-error/server-error.component')
      .then(m => m.ServerErrorComponent)
  },
  {
    path: ROUTE_PATHS.NOT_FOUND,
    loadComponent: () => import('./presentation/pages/errors/not-found/not-found.component')
      .then(m => m.NotFoundComponent)
  },

  // Wildcard Fallback Route to 404 Page
  {
    path: '**',
    redirectTo: ROUTE_PATHS.NOT_FOUND,
    pathMatch: 'full'
  }
];
