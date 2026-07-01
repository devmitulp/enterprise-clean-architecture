import { Injectable } from '@angular/core';
import { Injector, inject } from '@shared/angular';
// ==========================================================================
// AUTH TOKEN & CLAIMS SERVICE (Clean Architecture)
// ==========================================================================

import { lastValueFrom } from 'rxjs';
import { AUTH_STORAGE_KEYS, JWT_CLAIM_KEYS } from '@auth';
import { API_ENDPOINTS } from '@constants';
import { AppConfigService } from '@configuration';
import { BaseHttpService } from '@services';

@Injectable({
  providedIn: 'root',
})
export class AuthTokenService {
  private appConfig = inject(AppConfigService);
  private injector = inject(Injector);

  // --- Token Storage Management ---

  public getAccessToken(): string | null {
    return localStorage.getItem(AUTH_STORAGE_KEYS.ACCESS_TOKEN);
  }

  public setAccessToken(token: string): void {
    localStorage.setItem(AUTH_STORAGE_KEYS.ACCESS_TOKEN, token);
  }

  public getRefreshToken(): string | null {
    return localStorage.getItem(AUTH_STORAGE_KEYS.REFRESH_TOKEN);
  }

  public setRefreshToken(token: string): void {
    localStorage.setItem(AUTH_STORAGE_KEYS.REFRESH_TOKEN, token);
  }

  public clearTokens(): void {
    localStorage.removeItem(AUTH_STORAGE_KEYS.ACCESS_TOKEN);
    localStorage.removeItem(AUTH_STORAGE_KEYS.REFRESH_TOKEN);
    localStorage.removeItem(AUTH_STORAGE_KEYS.USER_PERMISSIONS);
  }

  // --- JWT Token Parsing & Verification ---

  public parseJwt(token: string): Record<string, any> | null {
    try {
      const base64Url = token.split('.')[1];
      const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
      const jsonPayload = decodeURIComponent(
        window
          .atob(base64)
          .split('')
          .map((c) => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
          .join('')
      );
      return JSON.parse(jsonPayload);
    } catch (e) {
      console.error('[AuthTokenService] Failed to parse JWT token', e);
      return null;
    }
  }

  public isAuthenticated(): boolean {
    const token = this.getAccessToken();
    if (!token) return false;

    const decoded = this.parseJwt(token);
    if (!decoded || !decoded[JWT_CLAIM_KEYS.EXP]) return false;

    // Check if token is expired (adding 30 seconds buffer)
    const expirationTime = decoded[JWT_CLAIM_KEYS.EXP] * 1000;
    return Date.now() < expirationTime - 30000;
  }

  // --- C# .NET Claims Extraction ---

  public getClaims(): Record<string, any> | null {
    const token = this.getAccessToken();
    return token ? this.parseJwt(token) : null;
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
      this.clearTokens();
      return false;
    }

    try {
      const http = this.injector.get(BaseHttpService);
      const response = await lastValueFrom(
        http.post<{ accessToken: string | null; refreshToken: string }, { accessToken: string; refreshToken: string }>(
          API_ENDPOINTS.AUTH.REFRESH,
          { accessToken, refreshToken }
        )
      );

      if (response && response.accessToken && response.refreshToken) {
        this.setAccessToken(response.accessToken);
        this.setRefreshToken(response.refreshToken);
        return true;
      }

      this.clearTokens();
      return false;
    } catch (error) {
      console.error('[AuthTokenService] Token refresh failed', error);
      this.clearTokens();
      return false;
    }
  }
}
