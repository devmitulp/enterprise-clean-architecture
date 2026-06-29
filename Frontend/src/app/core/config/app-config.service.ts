// ==========================================================================
// RUNTIME CONFIGURATION SERVICE (Clean Architecture APP_INITIALIZER)
// ==========================================================================

import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { AppConfig } from './app-config.interface';

@Injectable({
  providedIn: 'root',
})
export class AppConfigService {
  private config!: AppConfig;

  /**
   * Loads the runtime configuration JSON file before Angular bootstrap.
   * Using native fetch prevents circular dependency issues with HTTP Interceptors during startup.
   */
  public async loadConfig(): Promise<void> {
    const configPath = `./${environment.appSettingsPath}`;
    try {
      const response = await fetch(configPath);
      if (!response.ok) {
        throw new Error(`Failed to load appsettings file at ${configPath}: ${response.statusText}`);
      }
      this.config = await response.json();
      console.info(
        `[AppConfigService] Successfully loaded configuration for environment: ${environment.environmentName}`,
      );
    } catch (error) {
      console.error(
        `[AppConfigService] Critical Error: Configuration loading failed for environment: ${environment.environmentName}`,
        error,
      );
      throw error;
    }
  }

  // --- Strongly Typed Getters ---

  public get settings(): AppConfig {
    return this.config;
  }

  public get apiBaseUrl(): string {
    return this.config.apiBaseUrl;
  }

  public get featureFlags() {
    return this.config.featureFlags;
  }
}
