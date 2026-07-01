// ==========================================================================
// AUTH GUARD (Clean Architecture)
// ==========================================================================

import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AuthTokenService } from '@auth';
import { ROUTE_PATHS } from '@constants';

export const authGuard: CanActivateFn = (route, state) => {
  const authTokenService = inject(AuthTokenService);
  const router = inject(Router);

  if (authTokenService.isAuthenticated()) {
    return true;
  }

  // Not authenticated, redirect to login page
  authTokenService.clearTokens();
  return router.createUrlTree([ROUTE_PATHS.LOGIN]);
};
