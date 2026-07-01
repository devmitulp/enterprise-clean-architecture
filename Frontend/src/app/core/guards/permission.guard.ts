import { CanActivateFn, Router, inject } from '@shared/angular';
// ==========================================================================
// PERMISSION & ROLE GUARD FACTORY (Clean Architecture RBAC)
// ==========================================================================

import { AuthTokenService } from '@auth';
import { ROUTE_PATHS } from '@constants';

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
