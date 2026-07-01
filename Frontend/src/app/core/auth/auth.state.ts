import { Injectable } from '@angular/core';
import { Router, computed, inject, signal } from '@shared/angular';
// ==========================================================================
// MODERN ANGULAR SIGNAL STATE STORE (Clean Architecture Core)
// ==========================================================================

import { AuthTokenService } from '@auth';
import { JWT_CLAIM_KEYS } from '@auth';
import { ROUTE_PATHS } from '@constants';
import { TokenStorageService } from './token-storage.service';

@Injectable({
  providedIn: 'root',
})
export class AuthState {
  private authTokenService = inject(AuthTokenService);
  private tokenStorage = inject(TokenStorageService);
  private router = inject(Router);

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
  public currentUser = computed(() => this._currentUser());

  /** Read-only signal representing whether user is valid and authenticated */
  public isAuthenticated = computed(() => {
    const user = this._currentUser();
    if (!user || !user[JWT_CLAIM_KEYS.EXP]) return false;
    const expirationTime = user[JWT_CLAIM_KEYS.EXP] * 1000;
    return Date.now() < expirationTime - 30000;
  });

  /** Read-only signal representing global loading state */
  public isLoading = computed(() => this._isLoading());

  /** Read-only signal extracting user email from C# .NET XML or standard claims */
  public userEmail = computed(() => {
    const claims = this._currentUser();
    if (!claims) return null;
    return claims[JWT_CLAIM_KEYS.EMAIL_XML] || claims[JWT_CLAIM_KEYS.EMAIL] || null;
  });

  /** Read-only signal extracting user roles array from C# .NET XML or standard claims */
  public userRoles = computed<string[]>(() => {
    const claims = this._currentUser();
    if (!claims) return [];
    const roles = claims[JWT_CLAIM_KEYS.ROLE_XML] || claims[JWT_CLAIM_KEYS.ROLE] || [];
    return Array.isArray(roles) ? roles : [roles];
  });

  /** Read-only signal extracting tenant ID */
  public tenantId = computed(() => {
    const claims = this._currentUser();
    if (!claims) return null;
    return claims[JWT_CLAIM_KEYS.TENANT_ID] || null;
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
   * Dispatches logout action, clears storage and resets signal state to null.
   */
  public logout(): void {
    this.tokenStorage.clearTokens();
    this._currentUser.set(null);
    this.router.navigate([ROUTE_PATHS.LOGIN]);
  }

  /**
   * Explicitly sets global loading state.
   */
  public setLoading(loading: boolean): void {
    this._isLoading.set(loading);
  }
}
