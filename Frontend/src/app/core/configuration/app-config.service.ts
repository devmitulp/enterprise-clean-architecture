import { Injectable } from '@angular/core';
// ==========================================================================
// RUNTIME CONFIGURATION SERVICE (Clean Architecture APP_INITIALIZER)
// ==========================================================================

import { environment } from '@environment';
import { AppConfig } from './app-config.interface';

@Injectable({
  providedIn: 'root',
})
export class AppConfigService {
  private config!: AppConfig;
  private _isMaintenanceMode = false;

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

      // --- Startup API Health & Maintenance Check ---
      try {
        const apiBase = this.config.apiBaseUrl.replace(/\/+$/, '');
        const healthResponse = await fetch(`${apiBase}/common/settings`, { method: 'GET' });
        if (!healthResponse.ok) {
          throw new Error(`API health check returned status ${healthResponse.status}`);
        }
      } catch (apiError) {
        console.error(
          '[AppConfigService] API is unreachable or under maintenance. Enabling maintenance mode.',
          apiError
        );
        this._isMaintenanceMode = true;
      }
    } catch (error) {
      console.error(
        `[AppConfigService] Critical Error: Configuration loading failed for environment: ${environment.environmentName}`,
        error,
      );
      throw error;
    }
  }

  // --- Strongly Typed Getters ---

  public get isMaintenanceMode(): boolean {
    return this._isMaintenanceMode;
  }

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
