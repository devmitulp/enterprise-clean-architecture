// ==========================================================================
// STARTUP API HEALTH & MAINTENANCE ROUTE GUARDS (Clean Architecture)
// ==========================================================================

import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AppConfigService } from '@configuration';
import { ROUTE_PATHS } from '@constants';

/**
 * Validates system availability before granting access to layout routes.
 * Redirects dynamically to the /maintenance route if the API is offline.
 */
export const healthGuard: CanActivateFn = (route, state) => {
  const appConfig = inject(AppConfigService);
  const router = inject(Router);

  if (appConfig.isMaintenanceMode) {
    return router.createUrlTree([ROUTE_PATHS.MAINTENANCE]);
  }

  return true;
};

/**
 * Prevents access to the /maintenance route if the system is fully online.
 * Redirects the user back to the application home route.
 */
export const maintenanceGuard: CanActivateFn = (route, state) => {
  const appConfig = inject(AppConfigService);
  const router = inject(Router);

  if (!appConfig.isMaintenanceMode) {
    return router.createUrlTree([ROUTE_PATHS.HOME]);
  }

  return true;
};
