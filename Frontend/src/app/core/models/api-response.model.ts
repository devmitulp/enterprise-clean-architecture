// ==========================================================================
// C# .NET STANDARD RESPONSE MODELS (Clean Architecture Domain)
// ==========================================================================

/**
 * Standard Generic API Response Wrapper.
 * Commonly used in ASP.NET Core Web API for wrapping entity results.
 */
export interface ApiResponse<T = any> {
  data: T;
  succeeded: boolean;
  messages?: string[];
  statusCode: number;
}

/**
 * Standard Paginated Response Wrapper.
 * Commonly used in Entity Framework Core / Clean Architecture list queries.
 */
export interface PaginatedResponse<T> {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  totalCount: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}

/**
 * Official RFC 7807 Problem Details Specification.
 * Used by ASP.NET Core Web API for returning bad requests, validation errors, and unhandled exceptions.
 */
export interface ProblemDetails {
  type?: string;
  title?: string;
  status?: number;
  detail?: string;
  instance?: string;
  /**
   * Validation errors dictionary produced by ASP.NET Core ModelState or FluentValidation.
   * Example: { "Email": ["Email is required", "Invalid email format"] }
   */
  errors?: Record<string, string[]>;
  [key: string]: any; // Allows for additional problem detail extensions
}
