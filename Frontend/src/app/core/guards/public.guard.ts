// ==========================================================================
// PUBLIC GUARD (Clean Architecture)
// ==========================================================================

import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AuthTokenService } from '../services/auth-token.service';
import { ROUTE_PATHS } from '../constants/routes.constants';

export const publicGuard: CanActivateFn = (route, state) => {
  const authTokenService = inject(AuthTokenService);
  const router = inject(Router);

  // If already authenticated, prevent access to public/auth pages like login
  if (authTokenService.isAuthenticated()) {
    return router.createUrlTree([ROUTE_PATHS.DASHBOARD]);
  }

  return true;
};
