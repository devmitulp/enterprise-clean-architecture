import { HttpErrorResponse, HttpHandlerFn, HttpInterceptorFn, HttpRequest, Router, inject } from '@shared/angular';
// ==========================================================================
// GLOBAL ERROR & REFRESH INTERCEPTOR (Clean Architecture)
// ==========================================================================

import { throwError, from } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { AuthTokenService } from '@auth';
import { ROUTE_PATHS } from '@constants';
import { LoggerService } from '@services';

export const errorInterceptor: HttpInterceptorFn = (req: HttpRequest<unknown>, next: HttpHandlerFn) => {
  const authTokenService = inject(AuthTokenService);
  const router = inject(Router);
  const logger = inject(LoggerService);

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
      logger.error('[ErrorInterceptor] API Request Failed:', error.message || error);
      return throwError(() => error);
    })
  );
};
