// ==========================================================================
// RUNTIME CONFIGURATION INTERFACE (Clean Architecture)
// ==========================================================================

export interface AppConfig {
  apiBaseUrl: string;
  logging: {
    console: boolean;
    minLogLevel: 'Debug' | 'Information' | 'Warning' | 'Error';
  };
  featureFlags: {
    enableMultiFactorAuth: boolean;
    enableBetaDashboard: boolean;
  };
}
