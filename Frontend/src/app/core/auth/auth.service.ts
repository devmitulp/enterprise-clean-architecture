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

  logoutApi(): Observable<void> {
    const refreshToken = this.authTokenService.getRefreshToken() || '';
    return this.http.post<{ RefreshToken: string }, void>(API_ENDPOINTS.AUTH.LOGOUT, { RefreshToken: refreshToken });
  }
}
