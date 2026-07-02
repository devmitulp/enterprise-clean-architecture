import { ReactiveFormsModule } from '@angular/forms';

export const SHARED_ANGULAR_MODULES = [
  ReactiveFormsModule
];

// Value Exports
export {
  inject,
  signal,
  computed,
  ChangeDetectorRef,
  DestroyRef,
  Injector
} from '@angular/core';

export {
  FormBuilder,
  FormGroup,
  Validators,
  FormControl,
  NgControl
} from '@angular/forms';

export {
  Router
} from '@angular/router';

export {
  takeUntilDestroyed
} from '@angular/core/rxjs-interop';

export {
  HttpClient,
  HttpHeaders,
  HttpParams
} from '@angular/common/http';

// Type Exports
export type {
  OnInit
} from '@angular/core';

export type {
  AbstractControl,
  ControlValueAccessor
} from '@angular/forms';

export type {
  Routes,
  CanActivateFn
} from '@angular/router';

export type {
  HttpErrorResponse,
  HttpHandlerFn,
  HttpInterceptorFn,
  HttpRequest
} from '@angular/common/http';
