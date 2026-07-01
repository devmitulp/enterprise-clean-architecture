import {
  ApplicationConfig,
  inject,
  provideAppInitializer,
  provideBrowserGlobalErrorListeners,
  provideHttpClient,
  provideRouter,
  provideZonelessChangeDetection,
  withInterceptors,
} from '@shared/angular';
import { providePrimeNG } from '@primeng';
import Aura from '@primeuix/themes/aura';
import { provideTranslateService, provideTranslateLoader } from '@ngx-translate/core';
import { ApiTranslationLoader } from '@core';

import { routes } from './app.routes';
import { AppConfigService } from '@configuration';
import { authInterceptor } from '@auth';
import { errorInterceptor } from '@interceptors';
import { loaderInterceptor } from '@interceptors';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZonelessChangeDetection(),
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideHttpClient(withInterceptors([loaderInterceptor, authInterceptor, errorInterceptor])),
    provideAppInitializer(() => {
      const appConfigService = inject(AppConfigService);
      return appConfigService.loadConfig();
    }),
    provideTranslateService({
      lang: 'en',
      fallbackLang: 'en',
      loader: provideTranslateLoader(ApiTranslationLoader),
    }),
    providePrimeNG({
      theme: {
        preset: Aura,
        options: {
          darkModeSelector: '.dark',
        },
      },
    }),
  ],
};
