import { Injectable, inject, computed, signal } from '@angular/core';
import { Router } from '@angular/router';
// ==========================================================================
// MODERN ANGULAR SIGNAL STATE STORE (Clean Architecture Core)
// ==========================================================================

import { AuthTokenService, JWT_CLAIM_KEYS, UserContext } from '@auth';
import { ROUTE_PATHS } from '@constants';
import { TokenStorageService } from './token-storage.service';

@Injectable({
  providedIn: 'root',
})
export class AuthState {
  private readonly authTokenService = inject(AuthTokenService);
  private readonly tokenStorage = inject(TokenStorageService);
  private readonly router = inject(Router);

  // --- Private Writable Signals ---

  private _currentUser = signal<Record<string, any> | null>(this.getInitialClaims());
  private _isLoading = signal<boolean>(false);

  /**
   * Helper to initialize claims from local storage on first application load.
   */
  private getInitialClaims(): Record<string, any> | null {
    return this.authTokenService.isAuthenticated() ? this.authTokenService.getClaims() : null;
  }

  // --- Public Computed Signals (Read-Only) ---

  /** Read-only signal representing current user claims object */
  public readonly currentUser = computed(() => this._currentUser());

  /** Read-only signal representing whether user is valid and authenticated */
  public readonly isAuthenticated = computed(() => {
    const user = this._currentUser();
    if (!user) return false;
    return this.authTokenService.isAuthenticated();
  });

  /** Read-only signal representing global loading state */
  public readonly isLoading = computed(() => this._isLoading());

  /** Read-only signal representing the parsed UserContext from verified JWT claims */
  public readonly userContext = computed<UserContext | null>(() => {
    return this.fromClaims(this._currentUser());
  });

  /**
   * Helper mapping raw JWT claims into a structured UserContext interface.
   */
  private fromClaims(claims: Record<string, any> | null): UserContext | null {
    if (!claims) return null;

    const email = claims[JWT_CLAIM_KEYS.EMAIL_XML] || claims[JWT_CLAIM_KEYS.EMAIL] || null;
    const name = claims[JWT_CLAIM_KEYS.NAME_XML] || claims[JWT_CLAIM_KEYS.NAME] || null;
    const id = claims[JWT_CLAIM_KEYS.NAME_IDENTIFIER] || claims[JWT_CLAIM_KEYS.SUB] || null;
    const rolesData = claims[JWT_CLAIM_KEYS.ROLE_XML] || claims[JWT_CLAIM_KEYS.ROLE] || [];
    const roles = Array.isArray(rolesData) ? rolesData : [rolesData];
    const permissions = claims[JWT_CLAIM_KEYS.PERMISSIONS] || [];
    const permissionArray = Array.isArray(permissions) ? permissions : [permissions];
    const tenantId = claims[JWT_CLAIM_KEYS.TENANT_ID] || null;
    const department = claims[JWT_CLAIM_KEYS.DEPARTMENT] || null;

    let firstName =
      claims['given_name'] ||
      claims['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname'] ||
      null;
    let lastName =
      claims['family_name'] ||
      claims['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname'] ||
      null;

    if (!firstName && name) {
      const parts = name.trim().split(/\s+/);
      firstName = parts[0] || null;
      lastName = parts.slice(1).join(' ') || null;
    }

    return {
      Id: id,
      UserName: name || email || 'User',
      EmailAddress: email,
      FirstName: firstName,
      LastName: lastName,
      FullName: name || 'User',
      Roles: roles,
      Permissions: permissionArray,
      TenantId: tenantId,
      Department: department,
    };
  }

  /** Read-only signal extracting user email from the user context */
  public readonly userEmail = computed(() => this.userContext()?.EmailAddress ?? null);

  /** Read-only signal extracting user roles array from the user context */
  public readonly userRoles = computed(() => this.userContext()?.Roles ?? []);

  /** Read-only signal extracting tenant ID from the user context */
  public readonly tenantId = computed(() => this.userContext()?.TenantId ?? null);

  /** Read-only signal extracting user full name from the user context */
  public readonly userName = computed(() => this.userContext()?.FullName ?? 'User');

  /** Read-only signal generating initials from user full name */
  public readonly userInitials = computed(() => {
    const name = this.userName();
    if (!name) return 'U';
    const parts = name.trim().split(/\s+/);
    if (parts.length >= 2) {
      return (parts[0].charAt(0) + parts[parts.length - 1].charAt(0)).toUpperCase();
    }
    return name.slice(0, 2).toUpperCase();
  });

  // --- State Action Methods ---

  /**
   * Dispatches login success action, updates storage and updates signals reactively.
   */
  public loginSuccess(accessToken: string, refreshToken: string): void {
    this.tokenStorage.setAccessToken(accessToken);
    this.tokenStorage.setRefreshToken(refreshToken);
    const claims = this.tokenStorage.parseJwt(accessToken);
    this._currentUser.set(claims);
  }

  /**
   * Updates user context state in memory (e.g., after a token refresh).
   */
  public updateUserContext(userContext: UserContext): void {
    const token = this.tokenStorage.getAccessToken();
    if (token) {
      const claims = this.tokenStorage.parseJwt(token);
      this._currentUser.set(claims);
    }
  }

  /**
   * Dispatches logout action, clears storage and resets signal state to null.
   */
  public logout(): void {
    this.tokenStorage.clearTokens();
    this._currentUser.set(null);
    this.router.navigate([`/${ROUTE_PATHS.LOGIN}`]);
  }

  /**
   * Explicitly sets global loading state.
   */
  public setLoading(loading: boolean): void {
    this._isLoading.set(loading);
  }

  public hasRole(requiredRole: string): boolean {
    return this.userRoles().includes(requiredRole);
  }

  public hasPermission(requiredPermission: string): boolean {
    const permissions = this.userContext()?.Permissions ?? [];
    return permissions.includes(requiredPermission);
  }
}
