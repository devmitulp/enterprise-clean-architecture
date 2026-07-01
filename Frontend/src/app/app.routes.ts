import { Routes } from '@shared/angular';
import { ROUTE_PATHS } from '@constants';
import { PUBLIC_ROUTES } from '@pages';
import { AUTH_ROUTES } from '@pages';
import { PRIVATE_ROUTES } from '@pages';
import { ERROR_ROUTES } from '@pages';

export const routes: Routes = [
  // Error Pages (Directly loaded, no layout wrap required)
  ...ERROR_ROUTES,
  // Public Section (Before Sign-in General Pages: Home, About, Contact, Privacy, Terms)
  ...PUBLIC_ROUTES,
  // Auth Section (Before Sign-in Auth Flows: Login, Forgot Password, Reset Password, MFA)
  ...AUTH_ROUTES,
  // Private Section (After Sign-in Authenticated Workspace: Dashboard, Settings, etc.)
  ...PRIVATE_ROUTES,

  // Wildcard Fallback Route to 404 Page
  {
    path: '**',
    redirectTo: ROUTE_PATHS.NOT_FOUND,
    pathMatch: 'full',
  },
];
