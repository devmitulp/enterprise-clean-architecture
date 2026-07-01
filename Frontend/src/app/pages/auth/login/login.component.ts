import { ChangeDetectionStrategy, Component } from '@angular/core';
import { DestroyRef, FormBuilder, FormGroup, Router, SHARED_ANGULAR_MODULES, Validators, inject, signal, takeUntilDestroyed } from '@shared/angular';
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

  // Strongly-typed reactive form
  readonly loginForm: FormGroup = this.fb.group({
    Email: ['', [Validators.required, Validators.email]],
    Password: ['', [Validators.required, Validators.minLength(6)]],
    RememberMe: [false],
  });

  get emailControl() {
    return this.loginForm.get('Email');
  }
  get passwordControl() {
    return this.loginForm.get('Password');
  }

  onSubmit(): void {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    this.loginError.set(null);

    const request = this.loginForm.value as LoginRequest;

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
