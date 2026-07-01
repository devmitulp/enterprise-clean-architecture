// ==========================================================================
// GLOBAL ERROR & REFRESH INTERCEPTOR (Clean Architecture)
// ==========================================================================

import { HttpInterceptorFn, HttpRequest, HttpHandlerFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { throwError, from } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { AuthTokenService } from '@auth';
import { ROUTE_PATHS } from '@constants';

export const errorInterceptor: HttpInterceptorFn = (req: HttpRequest<unknown>, next: HttpHandlerFn) => {
  const authTokenService = inject(AuthTokenService);
  const router = inject(Router);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      // 1. Handle 401 Unauthorized (Trigger Silent Refresh Token flow)
      if (error.status === 401 && !req.url.includes('/auth/login') && !req.url.includes('/auth/refresh')) {
        return from(authTokenService.refreshToken()).pipe(
          switchMap((refreshed: boolean) => {
            if (refreshed) {
              const newToken = authTokenService.getAccessToken();
              const clonedRequest = req.clone({
                headers: req.headers.set('Authorization', `Bearer ${newToken}`),
              });
              return next(clonedRequest);
            } else {
              authTokenService.clearTokens();
              router.navigate([ROUTE_PATHS.LOGIN]);
              return throwError(() => error);
            }
          })
        );
      }

      // 2. Handle 403 Forbidden (Access Denied / Insufficient Permissions)
      if (error.status === 403) {
        router.navigate([ROUTE_PATHS.ACCESS_DENIED]);
        return throwError(() => error);
      }

      // 3. Handle 500 Internal Server Error & Others
      console.error('[ErrorInterceptor] API Request Failed:', error.message || error);
      return throwError(() => error);
    })
  );
};
