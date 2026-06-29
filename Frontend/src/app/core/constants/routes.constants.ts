/**
 * Centralized Route Paths for the Enterprise application.
 * Prevents hardcoding of path strings across components, services, and tests.
 */
export const ROUTE_PATHS = {
  // Public Layout Pages
  LOGIN: 'login',
} as const;

/**
 * Navigation helper to build routes programmatically.
 */
export const APP_ROUTES = {
  // Public
  login: () => `/${ROUTE_PATHS.LOGIN}`,
} as const;
