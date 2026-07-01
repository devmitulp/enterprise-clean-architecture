import { ChangeDetectionStrategy, Component } from '@angular/core';
import { FormBuilder, FormGroup, Router, SHARED_ANGULAR_MODULES, Validators, inject, signal } from '@shared/angular';
import { RouterLink } from '@angular/router';
import { ROUTE_PATHS } from '@constants';
import { TextBoxComponent } from '@form-controls';
import { TranslatePipe } from '@ngx-translate/core';
import { LoggerService } from '@services';

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

  readonly routePaths = ROUTE_PATHS;
  readonly isSubmitting = signal(false);
  readonly mfaError = signal<string | null>(null);

  // Strongly-typed reactive form for 6-digit MFA code
  readonly mfaForm: FormGroup = this.fb.group({
    code: ['', [Validators.required, Validators.pattern(/^\d{6}$/)]],
  });

  get codeControl() {
    return this.mfaForm.get('code');
  }

  onSubmit(): void {
    if (this.mfaForm.invalid) {
      this.mfaForm.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    this.mfaError.set(null);

    // Simulate MFA verification API delay
    setTimeout(() => {
      this.isSubmitting.set(false);
      this.logger.log('Successfully verified MFA code (mock).');
      this.router.navigate([`/${this.routePaths.DASHBOARD}`]);
    }, 1500);
  }

  resendCode(): void {
    this.logger.log('Resending MFA code...');
    alert('A new verification code has been sent to your registered device.');
  }
}
