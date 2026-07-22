// ==========================================================================
// API ENDPOINTS CONSTANTS (Clean Architecture Master Setup)
// ==========================================================================

/**
 * Centralized Single Source of Truth for all C# .NET Web API Endpoints.
 * Structured by Module / Domain to enforce clean separation of concerns.
 */
export const API_ENDPOINTS = {
  // --- Auth & Security Module ---
  AUTH: {
    LOGIN: 'auth/login',
    REFRESH: 'auth/refresh',
    LOGOUT: 'auth/logout',
    FORGOT_PASSWORD: 'auth/forgot-password',
    RESET_PASSWORD: 'auth/reset-password',
    MFA_VERIFY: 'auth/mfa-verify',
    MFA_SETUP: 'auth/mfa/setup',
    MFA_ENABLE: 'auth/mfa/enable',
    MFA_DISABLE: 'auth/mfa/disable',
  },

  // --- Master Data & Configuration Module ---
  MASTERS: {
    DEPARTMENTS: 'masters/departments',
    LOCATIONS: 'masters/locations',
    CATEGORIES: 'masters/categories',
    CONFIGURATIONS: 'masters/configurations',
  },

  // --- Common / Shared Endpoints ---
  COMMON: {
    FILE_UPLOAD: 'common/upload',
    SETTINGS: 'common/settings',
    LOOKUPS: 'common/lookups',
  },

  // --- Localization Module ---
  LOCALIZATION: {
    RESOURCES: 'language',
  },

  // --- User & Role Management Module ---
  USERS: {
    BASE: 'users',
    ROLES: 'users/roles',
    PERMISSIONS: 'users/permissions',
  },

  // --- EHS / Core Business Modules (Example Structure) ---
  INCIDENTS: {
    BASE: 'incidents',
    WORKFLOW: 'incidents/workflow',
    INVESTIGATIONS: 'incidents/investigations',
  },
} as const;
