import { HttpHandlerFn, HttpInterceptorFn, HttpRequest, inject } from '@shared/angular';
// ==========================================================================
// JWT AUTH INTERCEPTOR (Clean Architecture)
// ==========================================================================

import { AuthTokenService } from '@auth';
import { AppConfigService } from '@configuration';

export const authInterceptor: HttpInterceptorFn = (req: HttpRequest<unknown>, next: HttpHandlerFn) => {
  const authTokenService = inject(AuthTokenService);
  const appConfigService = inject(AppConfigService);
  const token = authTokenService.getAccessToken();

  // Check if request is targeting our C# .NET Backend API (avoid sending tokens to external APIs)
  const isApiUrl = req.url.startsWith(appConfigService.apiBaseUrl);

  if (token && isApiUrl) {
    const clonedRequest = req.clone({
      headers: req.headers.set('Authorization', `Bearer ${token}`),
    });
    return next(clonedRequest);
  }

  return next(req);
};
