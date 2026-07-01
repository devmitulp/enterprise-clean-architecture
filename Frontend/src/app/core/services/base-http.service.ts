import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams, inject } from '@shared/angular';
// ==========================================================================
// GENERIC BASE HTTP SERVICE (Clean Architecture Infrastructure)
// ==========================================================================

import { Observable } from 'rxjs';
import { AppConfigService } from '@configuration';

@Injectable({
  providedIn: 'root',
})
export class BaseHttpService {
  protected http = inject(HttpClient);
  protected appConfig = inject(AppConfigService);

  /**
   * Constructs the full absolute API URL by combining apiBaseUrl with the relative endpoint path.
   * Handles trailing/leading slashes cleanly.
   */
  protected getFullUrl(endpoint: string): string {
    const baseUrl = this.appConfig.apiBaseUrl.replace(/\/+$/, '');
    const cleanEndpoint = endpoint.replace(/^\/+/, '');
    return `${baseUrl}/${cleanEndpoint}`;
  }

  /**
   * Helper to convert plain JS objects or HttpParams into valid HttpParams.
   */
  protected buildParams(params?: HttpParams | Record<string, any>): HttpParams {
    if (params instanceof HttpParams) {
      return params;
    }
    let httpParams = new HttpParams();
    if (params) {
      Object.keys(params).forEach((key) => {
        const value = params[key];
        if (value !== undefined && value !== null) {
          httpParams = httpParams.set(key, value.toString());
        }
      });
    }
    return httpParams;
  }

  // --- Generic HTTP Methods ---

  /**
   * Executes an HTTP GET request returning an Observable of type T.
   */
  public get<T>(
    endpoint: string,
    params?: HttpParams | Record<string, any>,
    headers?: HttpHeaders
  ): Observable<T> {
    const url = this.getFullUrl(endpoint);
    const httpParams = this.buildParams(params);
    return this.http.get<T>(url, { params: httpParams, headers });
  }

  /**
   * Executes an HTTP POST request returning an Observable of type R (Response).
   */
  public post<T, R = T>(
    endpoint: string,
    body: T,
    params?: HttpParams | Record<string, any>,
    headers?: HttpHeaders
  ): Observable<R> {
    const url = this.getFullUrl(endpoint);
    const httpParams = this.buildParams(params);
    // NO retry on POST — it is non-idempotent. Retrying causes duplicate server-side
    // effects (e.g. login called 3× per click). Error handling is done by errorInterceptor.
    return this.http.post<R>(url, body, { params: httpParams, headers });
  }

  /**
   * Executes an HTTP PUT request returning an Observable of type R (Response).
   */
  public put<T, R = T>(
    endpoint: string,
    body: T,
    params?: HttpParams | Record<string, any>,
    headers?: HttpHeaders
  ): Observable<R> {
    const url = this.getFullUrl(endpoint);
    const httpParams = this.buildParams(params);
    // NO retry on PUT — non-idempotent in practice; errors handled by errorInterceptor.
    return this.http.put<R>(url, body, { params: httpParams, headers });
  }

  /**
   * Executes an HTTP PATCH request returning an Observable of type R (Response).
   */
  public patch<T, R = T>(
    endpoint: string,
    body: T,
    params?: HttpParams | Record<string, any>,
    headers?: HttpHeaders
  ): Observable<R> {
    const url = this.getFullUrl(endpoint);
    const httpParams = this.buildParams(params);
    // NO retry on PATCH — non-idempotent; errors handled by errorInterceptor.
    return this.http.patch<R>(url, body, { params: httpParams, headers });
  }

  /**
   * Executes an HTTP DELETE request returning an Observable of type T.
   */
  public delete<T>(
    endpoint: string,
    params?: HttpParams | Record<string, any>,
    headers?: HttpHeaders
  ): Observable<T> {
    const url = this.getFullUrl(endpoint);
    const httpParams = this.buildParams(params);
    // NO retry on DELETE — non-idempotent; errors handled by errorInterceptor.
    return this.http.delete<T>(url, { params: httpParams, headers });
  }
}
