/**
 * Centralized Route Paths for the Enterprise application.
 * Prevents hardcoding of path strings across components, services, and tests.
 */
export const ROUTE_PATHS = {
  // Public Layout Pages
  HOME: '',
  ABOUT: 'about-us',
  CONTACT: 'contact-us',
  PRIVACY: 'privacy-policy',
  TERMS: 'terms-and-conditions',

  // Auth Pages (Before Sign-in)
  LOGIN: 'login',
  FORGOT_PASSWORD: 'forgot-password',
  RESET_PASSWORD: 'reset-password',
  MFA: 'mfa',

  // Private Layout Pages (Authenticated Workspace)
  DASHBOARD: 'dashboard',
  SETTINGS: 'settings',

  // Error Pages
  ACCESS_DENIED: 'access-denied',
  NOT_FOUND: '404',
  SERVER_ERROR: '500',
  MAINTENANCE: 'maintenance',
} as const;

/**
 * Navigation helper to build routes programmatically.
 */
export const APP_ROUTES = {
  // Public
  home: () => `/${ROUTE_PATHS.HOME}`,
  about: () => `/${ROUTE_PATHS.ABOUT}`,
  contact: () => `/${ROUTE_PATHS.CONTACT}`,
  privacy: () => `/${ROUTE_PATHS.PRIVACY}`,
  terms: () => `/${ROUTE_PATHS.TERMS}`,

  // Auth
  login: () => `/${ROUTE_PATHS.LOGIN}`,
  forgotPassword: () => `/${ROUTE_PATHS.FORGOT_PASSWORD}`,
  resetPassword: () => `/${ROUTE_PATHS.RESET_PASSWORD}`,
  mfa: () => `/${ROUTE_PATHS.MFA}`,

  // Private
  dashboard: () => `/${ROUTE_PATHS.DASHBOARD}`,
  settings: () => `/${ROUTE_PATHS.SETTINGS}`,

  // Errors
  unauthorized: () => `/${ROUTE_PATHS.ACCESS_DENIED}`,
  notFound: () => `/${ROUTE_PATHS.NOT_FOUND}`,
  serverError: () => `/${ROUTE_PATHS.SERVER_ERROR}`,
  maintenance: () => `/${ROUTE_PATHS.MAINTENANCE}`,
} as const;
