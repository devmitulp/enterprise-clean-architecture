import { Injectable } from '@angular/core';
import { environment } from '@environment';

@Injectable({
  providedIn: 'root',
})
export class LoggerService {
  /**
   * Logs a debug message to the console.
   * Suppressed in production environment.
   */
  log(message: string, ...optionalParams: any[]): void {
    if (!environment.production) {
      console.log(message, ...optionalParams);
    }
  }

  /**
   * Logs an info message.
   * Suppressed in production environment.
   */
  info(message: string, ...optionalParams: any[]): void {
    if (!environment.production) {
      console.info(message, ...optionalParams);
    }
  }

  /**
   * Logs a warning.
   * Prints to console in development, and could be forwarded to APM (e.g., Sentry) in production.
   */
  warn(message: string, ...optionalParams: any[]): void {
    if (!environment.production) {
      console.warn(message, ...optionalParams);
    } else {
      // Production APM integration points here
    }
  }

  /**
   * Logs an error.
   * Prints to console in development, and forwards to APM (e.g., Sentry) in production.
   */
  error(message: string, ...optionalParams: any[]): void {
    if (!environment.production) {
      console.error(message, ...optionalParams);
    } else {
      // Production APM integration points here
    }
  }
}
