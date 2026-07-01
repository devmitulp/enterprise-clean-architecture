// ==========================================================================
// AUTHENTICATION & JWT CLAIMS CONSTANTS (Clean Architecture)
// ==========================================================================

export const AUTH_STORAGE_KEYS = {
  ACCESS_TOKEN: 'ehs_access_token',
  REFRESH_TOKEN: 'ehs_refresh_token',
  USER_PERMISSIONS: 'ehs_user_permissions',
} as const;

/**
 * Standard C# .NET / Microsoft Identity JWT Claim Keys
 */
export const JWT_CLAIM_KEYS = {
  // Microsoft Identity XML Schema Claims (Typical in ASP.NET Core Identity)
  NAME_IDENTIFIER: 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier',
  EMAIL_XML: 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress',
  NAME_XML: 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name',
  ROLE_XML: 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role',

  // Standard OpenID Connect / JWT Claims
  SUB: 'sub',
  EMAIL: 'email',
  NAME: 'name',
  ROLE: 'role',
  JTI: 'jti',
  EXP: 'exp',
  ISS: 'iss',
  AUD: 'aud',

  // Custom Enterprise EHS Claims
  TENANT_ID: 'tenantId',
  PERMISSIONS: 'permissions',
  DEPARTMENT: 'department',
} as const;
