import { ChangeDetectionStrategy, Component } from '@angular/core';
import { DestroyRef, FormBuilder, FormControl, FormGroup, Router, SHARED_ANGULAR_MODULES, Validators, inject, signal, takeUntilDestroyed } from '@shared/angular';
import { TranslatePipe } from '@ngx-translate/core';
import { ROUTE_PATHS } from '@constants';
import { TextBoxComponent, PasswordComponent } from '@form-controls';
import { LoggerService } from '@services';
import { AuthService, AuthState } from '@auth';
import { AppConfigService } from '@configuration';
import { LoginRequest } from '@models';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    ...SHARED_ANGULAR_MODULES,
    TextBoxComponent,
    TranslatePipe,
    PasswordComponent
  ],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LoginComponent {
  private readonly fb = inject(FormBuilder);
  private readonly router = inject(Router);
  private readonly logger = inject(LoggerService);
  private readonly authService = inject(AuthService);
  private readonly authState = inject(AuthState);
  private readonly appConfig = inject(AppConfigService);
  private readonly destroyRef = inject(DestroyRef);

  readonly routePaths = ROUTE_PATHS;
  readonly isSubmitting = signal(false);
  readonly loginError = signal<string | null>(null);

  // Strongly-typed reactive form directly typed with Dto properties
  readonly loginForm = this.fb.group<{
    [K in keyof LoginRequest]: FormControl<LoginRequest[K]>;
  }>({
    Email: this.fb.nonNullable.control('', [Validators.required, Validators.email]),
    Password: this.fb.nonNullable.control('', [Validators.required, Validators.minLength(6)]),
    RememberMe: this.fb.nonNullable.control(false),
  });

  get emailControl() {
    return this.loginForm.controls.Email;
  }
  get passwordControl() {
    return this.loginForm.controls.Password;
  }

  onSubmit(): void {
    // ─── Guard: prevent duplicate submissions ──────────────────────────────
    // In zoneless Angular, the DOM `disabled` binding is not updated
    // synchronously, so rapid clicks can bypass it. Reading the signal value
    // directly IS synchronous and acts as a reliable in-memory lock.
    if (this.isSubmitting()) {
      return;
    }

    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    this.loginError.set(null);

    const request = this.loginForm.value as LoginRequest;

    // ─── Single subscription with takeUntilDestroyed ───────────────────────
    // Each call to onSubmit() would normally create a new subscription.
    // The guard above ensures only ONE subscription is ever active at a time.
    // The request flows through: authInterceptor → errorInterceptor → loaderInterceptor → API
    this.authService.login(request)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (response) => {
          this.isSubmitting.set(false);
          this.logger.log('Successfully logged in.');
          if (response.RequiresMfa) {
            this.router.navigate([`/${this.routePaths.MFA}`], {
              state: { mfaToken: response.MfaToken }
            });
          } else {
            this.authState.loginSuccess(response.AccessToken, response.RefreshToken);
            this.router.navigate([`/${this.routePaths.DASHBOARD}`]);
          }
        },
        error: (err) => {
          this.isSubmitting.set(false);
          this.loginError.set(err.error?.message || 'Login failed. Please check your credentials.');
          this.logger.error('[LoginComponent] Login error', err);
        }
      });
  }

  loginWithGoogle(): void {
    const baseUrl = this.appConfig.apiBaseUrl.replace(/\/+$/, '');
    window.location.href = `${baseUrl}/auth/login-google`;
  }

  loginWithMicrosoft(): void {
    const baseUrl = this.appConfig.apiBaseUrl.replace(/\/+$/, '');
    window.location.href = `${baseUrl}/auth/login-microsoft`;
  }
}
