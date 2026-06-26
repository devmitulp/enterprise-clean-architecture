/**
 * Centralized Route Paths for the Enterprise application.
 * Prevents hardcoding of path strings across components, services, and tests.
 */
export const ROUTE_PATHS = {
  // Public Layout Pages
  HOME: '',
  ABOUT: 'about-us',
  CONTACT: 'contact-us',
  LOGIN: 'login',
  FORGOT_PASSWORD: 'forgot-password',
  RESET_PASSWORD: 'reset-password',
  PRIVACY: 'privacy-policy',
  TERMS: 'terms-and-conditions',

  // Private Layout Pages (Authenticated Workspace)
  DASHBOARD: 'dashboard',
  SETTINGS: 'settings',

  // Error Pages
  UNAUTHORIZED: 'unauthorized',
  NOT_FOUND: '404',
  SERVER_ERROR: '500',
} as const;

/**
 * Navigation helper to build routes programmatically.
 */
export const APP_ROUTES = {
  // Public
  home: () => `/${ROUTE_PATHS.HOME}`,
  about: () => `/${ROUTE_PATHS.ABOUT}`,
  contact: () => `/${ROUTE_PATHS.CONTACT}`,
  login: () => `/${ROUTE_PATHS.LOGIN}`,
  forgotPassword: () => `/${ROUTE_PATHS.FORGOT_PASSWORD}`,
  resetPassword: () => `/${ROUTE_PATHS.RESET_PASSWORD}`,
  privacy: () => `/${ROUTE_PATHS.PRIVACY}`,
  terms: () => `/${ROUTE_PATHS.TERMS}`,

  // Private
  dashboard: () => `/${ROUTE_PATHS.DASHBOARD}`,
  settings: () => `/${ROUTE_PATHS.SETTINGS}`,

  // Errors
  unauthorized: () => `/${ROUTE_PATHS.UNAUTHORIZED}`,
  notFound: () => `/${ROUTE_PATHS.NOT_FOUND}`,
  serverError: () => `/${ROUTE_PATHS.SERVER_ERROR}`,
} as const;
