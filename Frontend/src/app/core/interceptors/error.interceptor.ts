import { HttpErrorResponse, HttpHandlerFn, HttpInterceptorFn, HttpRequest, Router, inject } from '@shared/angular';
// ==========================================================================
// GLOBAL ERROR & REFRESH INTERCEPTOR (Clean Architecture)
// ==========================================================================

import { throwError, from, BehaviorSubject } from 'rxjs';
import { catchError, switchMap, filter, take } from 'rxjs/operators';
import { AuthTokenService, AuthState } from '@auth';
import { ROUTE_PATHS } from '@constants';
import { LoggerService } from '@services';

// Concurrent refresh state locking
let isRefreshing = false;
const refreshTokenSubject = new BehaviorSubject<string | null>(null);

export const errorInterceptor: HttpInterceptorFn = (req: HttpRequest<unknown>, next: HttpHandlerFn) => {
  const authTokenService = inject(AuthTokenService);
  const authState = inject(AuthState);
  const router = inject(Router);
  const logger = inject(LoggerService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      // 1. Handle 401 Unauthorized (Trigger Silent Refresh Token flow)
      if (
        error.status === 401 &&
        !req.url.includes('/auth/login') &&
        !req.url.includes('/auth/refresh') &&
        !req.url.includes('/auth/mfa-verify')
      ) {
        if (!isRefreshing) {
          isRefreshing = true;
          refreshTokenSubject.next(null); // Reset subject to block concurrent requests

          return from(authTokenService.refreshToken()).pipe(
            switchMap((refreshed: boolean) => {
              isRefreshing = false;
              if (refreshed) {
                const newToken = authTokenService.getAccessToken();
                refreshTokenSubject.next(newToken); // Propagate token to waiting queue
                const clonedRequest = req.clone({
                  headers: req.headers.set('Authorization', `Bearer ${newToken}`),
                });
                return next(clonedRequest);
              } else {
                refreshTokenSubject.next(''); // Signal failure to queue
                authState.logout();
                return throwError(() => error);
              }
            }),
            catchError((refreshError) => {
              isRefreshing = false;
              refreshTokenSubject.next('');
              authState.logout();
              return throwError(() => refreshError);
            })
          );
        } else {
          // Refresh is in progress; queue request until non-null emission
          return refreshTokenSubject.pipe(
            filter((token) => token !== null),
            take(1),
            switchMap((token) => {
              if (token === '') {
                // Refresh failed for lock holder, propagate unauthorized error
                return throwError(() => error);
              }
              const clonedRequest = req.clone({
                headers: req.headers.set('Authorization', `Bearer ${token}`),
              });
              return next(clonedRequest);
            })
          );
        }
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
