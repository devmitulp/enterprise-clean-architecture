import { Injectable, inject } from '@angular/core';
import { BaseHttpService } from '../services/base-http.service';
import { Observable } from 'rxjs';
import { API_ENDPOINTS } from '../constants/api-endpoints.constants';
import { LoginRequest, LoginResponse, MfaVerifyRequest } from '@models';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private http = inject(BaseHttpService);

  login(credentials: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginRequest, LoginResponse>(API_ENDPOINTS.AUTH.LOGIN, credentials);
  }

  verifyMfa(request: MfaVerifyRequest): Observable<LoginResponse> {
    return this.http.post<MfaVerifyRequest, LoginResponse>(API_ENDPOINTS.AUTH.MFA_VERIFY, request);
  }

  logoutApi(): Observable<void> {
    return this.http.post<void, void>(API_ENDPOINTS.AUTH.LOGOUT, undefined);
  }
}
