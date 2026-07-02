import { Injectable, Inject, Renderer2, RendererFactory2, inject } from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { LocalStorageService } from './local-storage.service';

@Injectable({
  providedIn: 'root',
})
export class ThemeService {
  private readonly document = inject(DOCUMENT);
  private readonly rendererFactory = inject(RendererFactory2);
  private readonly storage = inject(LocalStorageService);
  private renderer: Renderer2;

  private isDark = false;

  constructor() {
    this.renderer = this.rendererFactory.createRenderer(null, null);
    // Initialize theme from storage (defaults to false/light if not set)
    const storedTheme = this.storage.getItem('theme');
    this.isDark = storedTheme === 'dark';
    this.applyTheme(this.isDark);
  }

  public isDarkMode(): boolean {
    return this.isDark;
  }

  public toggleTheme(): boolean {
    this.isDark = !this.isDark;
    this.storage.setItem('theme', this.isDark ? 'dark' : 'light');
    this.applyTheme(this.isDark);
    return this.isDark;
  }

  private applyTheme(isDark: boolean): void {
    const htmlElement = this.document.documentElement;
    if (isDark) {
      this.renderer.addClass(htmlElement, 'dark');
    } else {
      this.renderer.removeClass(htmlElement, 'dark');
    }
  }
}
