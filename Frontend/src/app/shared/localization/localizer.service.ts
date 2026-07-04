import { Injectable } from '@angular/core';
import { inject } from '@shared/angular';
import { LocalizationKey } from './localization.types';
import { TranslateService } from '@ngx-translate/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class LocalizerService {
  private readonly translate = inject(TranslateService);

  /**
   * Returns translated text.
   */
  text(key: LocalizationKey | string, ...parameters: unknown[]): string {
    let message = this.translate.instant(key);

    parameters.forEach((parameter, index) => {
      message = message.replace(`{${index}}`, String(parameter));
    });

    return message;
  }

  /**
   * Alias of text()
   */
  get(key: LocalizationKey | string, ...parameters: unknown[]): string {
    return this.text(key, ...parameters);
  }

  /**
   * Returns translated text if available; otherwise returns the key.
   */
  tryGet(key: LocalizationKey | string, ...parameters: unknown[]): string {
    return this.exists(key) ? this.text(key, ...parameters) : (key as string);
  }

  /**
   * Checks whether a localization key exists.
   */
  exists(key: LocalizationKey | string): boolean {
    const translateService = this.translate as any;
    const currentLang = translateService.currentLang();
    const translations = translateService.store?.translations[currentLang];
    if (!translations) {
      return false;
    }
    return translateService.parser.getValue(translations, key) !== undefined;
  }

  /**
   * Returns translated text asynchronously, waiting for translations to load if necessary.
   */
  getTextAsync(key: LocalizationKey | string, ...parameters: unknown[]): Observable<string> {
    return this.translate.get(key).pipe(
      map(message => {
        let formattedMessage = message;
        parameters.forEach((parameter, index) => {
          formattedMessage = formattedMessage.replace(`{${index}}`, String(parameter));
        });
        return formattedMessage;
      })
    );
  }
}
