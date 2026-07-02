import { HttpClient, inject } from '@shared/angular';
// ==========================================================================
// API TRANSLATION LOADER (Clean Architecture Infrastructure)
// ==========================================================================

import { TranslateLoader } from '@ngx-translate/core';
import { Observable } from 'rxjs';
import { AppConfigService } from '@configuration';
import { API_ENDPOINTS } from '@constants';

/**
 * Custom translation loader that fetches dictionary resources dynamically
 * from the C# Web API based on the selected culture/language code.
 */
export class ApiTranslationLoader implements TranslateLoader {
  private readonly http = inject(HttpClient);
  private readonly appConfig = inject(AppConfigService);

  public getTranslation(lang: string): Observable<Record<string, string>> {
    const baseUrl = this.appConfig.apiBaseUrl.replace(/\/+$/, '');
    const url = `${baseUrl}/${API_ENDPOINTS.LOCALIZATION.RESOURCES}/${lang}`;

    // We send 'culture' query parameter to let the backend middleware set the culture context
    return this.http.get<Record<string, string>>(url);
  }
}
