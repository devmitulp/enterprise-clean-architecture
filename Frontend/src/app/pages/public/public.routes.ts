import { Routes } from '@shared/angular';
import { ROUTE_PATHS } from '@constants';
import { PublicLayoutComponent } from '@layout';
import { healthGuard } from '@guards';

export const PUBLIC_ROUTES: Routes = [
  {
    path: '',
    component: PublicLayoutComponent,
    canActivate: [healthGuard],
    children: [
      {
        path: ROUTE_PATHS.HOME,
        loadComponent: () => import('./home/home.component').then((m) => m.HomeComponent),
      },
      {
        path: ROUTE_PATHS.ABOUT,
        loadComponent: () => import('./about/about.component').then((m) => m.AboutComponent),
      },
      {
        path: ROUTE_PATHS.CONTACT,
        loadComponent: () => import('./contact/contact.component').then((m) => m.ContactComponent),
      },
      {
        path: ROUTE_PATHS.PRIVACY,
        loadComponent: () => import('./privacy/privacy.component').then((m) => m.PrivacyComponent),
      },
      {
        path: ROUTE_PATHS.TERMS,
        loadComponent: () => import('./terms/terms.component').then((m) => m.TermsComponent),
      },
    ],
  },
];
