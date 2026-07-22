import { Injectable, inject } from '@angular/core';
import { BaseHttpService } from '../services/base-http.service';
import { Observable } from 'rxjs';
import { API_ENDPOINTS } from '../constants/api-endpoints.constants';
import { LoginRequest, LoginResponse, MfaVerifyRequest } from '@models';
import { AuthTokenService } from './auth-token.service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly http = inject(BaseHttpService);
  private readonly authTokenService = inject(AuthTokenService);

  login(credentials: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginRequest, LoginResponse>(API_ENDPOINTS.AUTH.LOGIN, credentials);
  }

  verifyMfa(request: MfaVerifyRequest): Observable<LoginResponse> {
    return this.http.post<MfaVerifyRequest, LoginResponse>(API_ENDPOINTS.AUTH.MFA_VERIFY, request);
  }

  setupMfa(): Observable<{ Secret: string; QrCodeSvg: string }> {
    return this.http.post<void, { Secret: string; QrCodeSvg: string }>(API_ENDPOINTS.AUTH.MFA_SETUP, undefined);
  }

  enableMfa(request: { Code: string; Secret: string }): Observable<{ RecoveryCodes: string[] }> {
    return this.http.post<{ Code: string; Secret: string }, { RecoveryCodes: string[] }>(API_ENDPOINTS.AUTH.MFA_ENABLE, request);
  }

  disableMfa(request: { Code: string }): Observable<any> {
    return this.http.post<{ Code: string }, any>(API_ENDPOINTS.AUTH.MFA_DISABLE, request);
  }

  logoutApi(): Observable<void> {
    const refreshToken = this.authTokenService.getRefreshToken() || '';
    return this.http.post<{ RefreshToken: string }, void>(API_ENDPOINTS.AUTH.LOGOUT, { RefreshToken: refreshToken });
  }
}
