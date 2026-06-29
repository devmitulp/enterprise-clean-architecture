// ==========================================================================
// PERMISSION & ROLE GUARD FACTORY (Clean Architecture RBAC)
// ==========================================================================

import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AuthTokenService } from '../services/auth-token.service';
import { ROUTE_PATHS } from '../constants/routes.constants';

/**
 * Factory function to create a CanActivateFn guard for checking specific user permissions/roles.
 */
export function canActivatePermission(requiredPermission: string): CanActivateFn {
  return (route, state) => {
    const authTokenService = inject(AuthTokenService);
    const router = inject(Router);

    if (!authTokenService.isAuthenticated()) {
      return router.createUrlTree([ROUTE_PATHS.LOGIN]);
    }

    if (authTokenService.hasPermission(requiredPermission) || authTokenService.hasRole(requiredPermission)) {
      return true;
    }

    // Access Denied / Insufficient Permissions
    return router.createUrlTree([ROUTE_PATHS.ACCESS_DENIED]);
  };
}
