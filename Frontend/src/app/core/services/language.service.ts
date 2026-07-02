import { Injectable, inject, computed } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Injectable({
  providedIn: 'root',
})
export class LanguageService {
  private readonly translate = inject(TranslateService);

  // Singleton computed signal representing the active language context
  public readonly currentLang = computed(() => this.translate.currentLang() || 'en');

  public setLanguage(code: string): void {
    this.translate.use(code);
  }
}
