import { HttpInterceptorFn, HttpRequest, HttpHandlerFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { LoaderService } from '@services';
import { finalize } from 'rxjs';

export const loaderInterceptor: HttpInterceptorFn = (req: HttpRequest<unknown>, next: HttpHandlerFn) => {
  const loaderService = inject(LoaderService);
  
  // Show loading indicator
  loaderService.show();
  
  return next(req).pipe(
    finalize(() => {
      // Hide loading indicator when request completes or fails
      loaderService.hide();
    })
  );
};
