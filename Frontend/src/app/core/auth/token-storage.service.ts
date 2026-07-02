import { Injectable, inject } from '@angular/core';
import { AUTH_STORAGE_KEYS } from './auth.constants';
import { LocalStorageService } from '@services';

@Injectable({
  providedIn: 'root',
})
export class TokenStorageService {
  private readonly localStorageService = inject(LocalStorageService);

  setAccessToken(token: string): void {
    this.localStorageService.setItem(AUTH_STORAGE_KEYS.ACCESS_TOKEN, token);
  }

  getAccessToken(): string | null {
    return this.localStorageService.getItem(AUTH_STORAGE_KEYS.ACCESS_TOKEN);
  }

  setRefreshToken(token: string): void {
    this.localStorageService.setItem(AUTH_STORAGE_KEYS.REFRESH_TOKEN, token);
  }

  getRefreshToken(): string | null {
    return this.localStorageService.getItem(AUTH_STORAGE_KEYS.REFRESH_TOKEN);
  }

  clearTokens(): void {
    this.localStorageService.removeItem(AUTH_STORAGE_KEYS.ACCESS_TOKEN);
    this.localStorageService.removeItem(AUTH_STORAGE_KEYS.REFRESH_TOKEN);
    this.localStorageService.removeItem(AUTH_STORAGE_KEYS.USER_PERMISSIONS);
  }

  parseJwt(token: string): Record<string, any> | null {
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
      return null;
    }
  }
}
