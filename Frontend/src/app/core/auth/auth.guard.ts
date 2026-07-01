import { CanActivateFn, inject } from '@shared/angular';
// ==========================================================================
// AUTH GUARD (Clean Architecture)
// ==========================================================================

import { AuthTokenService, AuthState } from '@auth';

export const authGuard: CanActivateFn = (route, state) => {
  const authTokenService = inject(AuthTokenService);
  const authState = inject(AuthState);

  if (authTokenService.isAuthenticated()) {
    return true;
  }

  // Not authenticated, redirect to login page via authState logout
  authState.logout();
  return false;
};
