// ==========================================================================
// API TRANSLATION LOADER (Clean Architecture Infrastructure)
// ==========================================================================

import { TranslateLoader } from '@ngx-translate/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AppConfigService } from '../config/app-config.service';
import { inject } from '@angular/core';
import { API_ENDPOINTS } from '../constants/api-endpoints.constants';

/**
 * Custom translation loader that fetches dictionary resources dynamically
 * from the C# Web API based on the selected culture/language code.
 */
export class ApiTranslationLoader implements TranslateLoader {
  private http = inject(HttpClient);
  private appConfig = inject(AppConfigService);

  public getTranslation(lang: string): Observable<Record<string, string>> {
    const baseUrl = this.appConfig.apiBaseUrl.replace(/\/+$/, '');
    const url = `${baseUrl}/${API_ENDPOINTS.LOCALIZATION.RESOURCES}/${lang}`;

    // We send 'culture' query parameter to let the backend middleware set the culture context
    return this.http.get<Record<string, string>>(url);
  }
}
