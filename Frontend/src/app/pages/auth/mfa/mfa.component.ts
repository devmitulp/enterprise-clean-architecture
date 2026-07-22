import { ChangeDetectionStrategy, Component, DestroyRef, inject, signal } from '@angular/core';
import { RouterLink, Router } from '@angular/router';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { AuthService, AuthState } from '@auth';
import { ROUTE_PATHS } from '@constants';
import { TextBoxComponent } from '@form-controls';
import { MfaVerifyRequest } from '@models';
import { TranslatePipe } from '@ngx-translate/core';
import { LoggerService } from '@services';
import { SHARED_ANGULAR_MODULES } from '@shared/angular';
import { LocalizerService } from '@shared/localization';

@Component({
  selector: 'app-mfa',
  standalone: true,
  imports: [...SHARED_ANGULAR_MODULES, TextBoxComponent, RouterLink, TranslatePipe],
  templateUrl: './mfa.component.html',
  styleUrl: './mfa.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MfaComponent {
  private readonly fb = inject(FormBuilder);
  private readonly router = inject(Router);
  private readonly logger = inject(LoggerService);
  private readonly authService = inject(AuthService);
  private readonly authState = inject(AuthState);
  private readonly destroyRef = inject(DestroyRef);
  private readonly sanitizer = inject(DomSanitizer);
  private readonly localizer = inject(LocalizerService);

  readonly routePaths = ROUTE_PATHS;
  readonly isSubmitting = signal(false);
  readonly mfaError = signal<string | null>(null);

  // Setup / Verify Step Signals
  readonly mfaToken = signal<string | null>(null);
  readonly isSetupRequired = signal(false);
  readonly secret = signal<string | null>(null);
  readonly qrCodeSvg = signal<SafeHtml | null>(null);

  // Controls whether the Verify Code input section is displayed
  readonly showVerifySection = signal(false);

  // Strongly-typed reactive form for 6-digit MFA code or recovery code
  readonly mfaForm = this.fb.group<{
    [K in keyof Pick<MfaVerifyRequest, 'Code'>]: FormControl<MfaVerifyRequest[K]>;
  }>({
    Code: this.fb.nonNullable.control('', [
      Validators.required,
      Validators.pattern(/^(\d{6}|[a-zA-Z0-9]{4}-[a-zA-Z0-9]{4})$/),
    ]),
  });

  constructor() {
    const navigation = this.router.getCurrentNavigation();
    const state = navigation?.extras.state as {
      mfaToken?: string;
      isSetupRequired?: boolean;
      qrCodeSvg?: string;
      secret?: string;
    };

    if (state?.mfaToken) {
      this.mfaToken.set(state.mfaToken);
      this.isSetupRequired.set(!!state.isSetupRequired);
      this.secret.set(state.secret || null);

      if (state.qrCodeSvg) {
        this.qrCodeSvg.set(this.sanitizer.bypassSecurityTrustHtml(state.qrCodeSvg));
      }

      // If user ALREADY scanned/set up QR code previously, jump straight to verification
      if (!state.isSetupRequired) {
        this.showVerifySection.set(true);
      } else {
        // First time setup: show QR code first!
        this.showVerifySection.set(false);
      }
    } else {
      this.router.navigate([`/${this.routePaths.LOGIN}`]);
    }
  }

  get codeControl() {
    return this.mfaForm.controls.Code;
  }

  onContinueToVerify(): void {
    // User clicked "Continue" after scanning the QR code -> reveal the Verify Code section!
    this.showVerifySection.set(true);
  }

  onBackToQrCode(): void {
    // Allow user to view QR code instructions again
    this.showVerifySection.set(false);
  }

  onSubmit(): void {
    if (this.mfaForm.invalid) {
      this.mfaForm.markAllAsTouched();
      return;
    }

    const token = this.mfaToken();
    if (!token) {
      this.mfaError.set(this.localizer.get('InvalidMfaToken'));
      return;
    }

    this.isSubmitting.set(true);
    this.mfaError.set(null);

    const code = this.mfaForm.value.Code || '';
    const request: MfaVerifyRequest = {
      Code: code,
      MfaToken: token,
      Secret: this.secret() ?? undefined,
    };

    this.authService
      .verifyMfa(request)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (response) => {
          this.isSubmitting.set(false);
          this.authState.loginSuccess(response.AccessToken!, response.RefreshToken!);
          this.router.navigate([`/${this.routePaths.DASHBOARD}`]);
        },
        error: (err) => {
          this.isSubmitting.set(false);
          const backendMessage = err.error?.Message || err.error?.message;
          this.mfaError.set(backendMessage || this.localizer.get('InvalidMfaCode'));
          this.logger.error('[MfaComponent] MFA verification error', err);
        },
      });
  }

  resendCode(): void {
    this.logger.log('Resending MFA code...');
  }
}
