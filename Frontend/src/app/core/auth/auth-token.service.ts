import { Injectable } from '@angular/core';
import { Injector, inject } from '@shared/angular';
// ==========================================================================
// AUTH TOKEN & CLAIMS SERVICE (Clean Architecture)
// ==========================================================================

import { lastValueFrom } from 'rxjs';
import { JWT_CLAIM_KEYS } from '@auth';
import { API_ENDPOINTS } from '@constants';
import { BaseHttpService, LoggerService } from '@services';
import { TokenStorageService } from './token-storage.service';

@Injectable({
  providedIn: 'root',
})
export class AuthTokenService {
  private injector = inject(Injector);
  private readonly logger = inject(LoggerService);
  private readonly tokenStorage = inject(TokenStorageService);

  // --- Token Storage Read Access ---

  public getAccessToken(): string | null {
    return this.tokenStorage.getAccessToken();
  }

  public getRefreshToken(): string | null {
    return this.tokenStorage.getRefreshToken();
  }

  public isAuthenticated(): boolean {
    const token = this.getAccessToken();
    if (!token) return false;

    const decoded = this.tokenStorage.parseJwt(token);
    if (!decoded || !decoded[JWT_CLAIM_KEYS.EXP]) return false;

    // Check if token is expired (adding 30 seconds buffer)
    const expirationTime = decoded[JWT_CLAIM_KEYS.EXP] * 1000;
    return Date.now() < expirationTime - 30000;
  }

  // --- C# .NET Claims Extraction ---

  public getClaims(): Record<string, any> | null {
    const token = this.getAccessToken();
    return token ? this.tokenStorage.parseJwt(token) : null;
  }

  public getUserEmail(): string | null {
    const claims = this.getClaims();
    if (!claims) return null;
    return claims[JWT_CLAIM_KEYS.EMAIL_XML] || claims[JWT_CLAIM_KEYS.EMAIL] || null;
  }

  public getUserRoles(): string[] {
    const claims = this.getClaims();
    if (!claims) return [];
    const roles = claims[JWT_CLAIM_KEYS.ROLE_XML] || claims[JWT_CLAIM_KEYS.ROLE] || [];
    return Array.isArray(roles) ? roles : [roles];
  }

  public hasRole(requiredRole: string): boolean {
    const roles = this.getUserRoles();
    return roles.includes(requiredRole);
  }

  public hasPermission(requiredPermission: string): boolean {
    const claims = this.getClaims();
    if (!claims) return false;
    const permissions = claims[JWT_CLAIM_KEYS.PERMISSIONS] || [];
    const permissionArray = Array.isArray(permissions) ? permissions : [permissions];
    return permissionArray.includes(requiredPermission);
  }

  // --- Silent Refresh Token Flow ---

  /**
   * Refreshes the JWT Access Token using the stored Refresh Token.
   * Uses Angular Injector at runtime to fetch BaseHttpService, preventing Circular Dependency DI loops.
   */
  public async refreshToken(): Promise<boolean> {
    const refreshToken = this.getRefreshToken();
    const accessToken = this.getAccessToken();
    if (!refreshToken) {
      this.tokenStorage.clearTokens();
      return false;
    }

    try {
      const http = this.injector.get(BaseHttpService);
      const response = await lastValueFrom(
        http.post<{ AccessToken: string | null; RefreshToken: string | null }, { AccessToken: string; RefreshToken: string }>(
          API_ENDPOINTS.AUTH.REFRESH,
          { AccessToken: accessToken, RefreshToken: refreshToken }
        )
      );

      if (response && response.AccessToken && response.RefreshToken) {
        this.tokenStorage.setAccessToken(response.AccessToken);
        this.tokenStorage.setRefreshToken(response.RefreshToken);
        return true;
      }

      this.tokenStorage.clearTokens();
      return false;
    } catch (error) {
      this.logger.error('[AuthTokenService] Token refresh failed', error);
      this.tokenStorage.clearTokens();
      return false;
    }
  }
}
