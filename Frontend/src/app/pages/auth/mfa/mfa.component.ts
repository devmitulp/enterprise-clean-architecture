import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { DestroyRef, FormBuilder, FormGroup, Router, SHARED_ANGULAR_MODULES, Validators, inject, signal, takeUntilDestroyed } from '@shared/angular';
import { RouterLink } from '@angular/router';
import { ROUTE_PATHS } from '@constants';
import { TextBoxComponent } from '@form-controls';
import { TranslatePipe } from '@ngx-translate/core';
import { LoggerService } from '@services';
import { AuthService, AuthState } from '@auth';
import { MfaVerifyRequest } from '@models';

@Component({
  selector: 'app-mfa',
  standalone: true,
  imports: [
    ...SHARED_ANGULAR_MODULES,
    TextBoxComponent,
    RouterLink,
    TranslatePipe
  ],
  templateUrl: './mfa.component.html',
  styleUrls: ['./mfa.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MfaComponent {
  private readonly fb = inject(FormBuilder);
  private readonly router = inject(Router);
  private readonly logger = inject(LoggerService);
  private readonly authService = inject(AuthService);
  private readonly authState = inject(AuthState);
  private readonly destroyRef = inject(DestroyRef);

  readonly routePaths = ROUTE_PATHS;
  readonly isSubmitting = signal(false);
  readonly mfaError = signal<string | null>(null);
  private readonly mfaToken = signal<string | null>(null);

  // Strongly-typed reactive form for 6-digit MFA code
  readonly mfaForm: FormGroup = this.fb.group({
    code: ['', [Validators.required, Validators.pattern(/^\d{6}$/)]],
  });

  constructor() {
    const navigation = this.router.getCurrentNavigation();
    const state = navigation?.extras.state as { mfaToken?: string };
    if (state?.mfaToken) {
      this.mfaToken.set(state.mfaToken);
    } else {
      this.router.navigate([`/${this.routePaths.LOGIN}`]);
    }
  }

  get codeControl() {
    return this.mfaForm.get('code');
  }

  onSubmit(): void {
    if (this.mfaForm.invalid) {
      this.mfaForm.markAllAsTouched();
      return;
    }

    const token = this.mfaToken();
    if (!token) {
      this.mfaError.set('MFA verification session expired. Please log in again.');
      return;
    }

    this.isSubmitting.set(true);
    this.mfaError.set(null);

    const code = this.mfaForm.value.code;
    const request: MfaVerifyRequest = {
      Code: code,
      MfaToken: token
    };

    this.authService.verifyMfa(request)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
      next: (response) => {
        this.isSubmitting.set(false);
        this.authState.loginSuccess(response.AccessToken, response.RefreshToken);
        this.router.navigate([`/${this.routePaths.DASHBOARD}`]);
      },
      error: (err) => {
        this.isSubmitting.set(false);
        this.mfaError.set(err.error?.message || 'Invalid verification code.');
        this.logger.error('[MfaComponent] MFA verification error', err);
      }
    });
  }

  resendCode(): void {
    this.logger.log('Resending MFA code...');
    alert('A new verification code has been sent to your registered device.');
  }
}
