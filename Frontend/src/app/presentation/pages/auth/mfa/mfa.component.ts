import { Component, ChangeDetectionStrategy, signal, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { ROUTE_PATHS } from '../../../../core/constants/routes.constants';
import { TextBoxComponent } from '../../../../shared/components/text-box/text-box.component';
import { TranslatePipe } from '@ngx-translate/core';

@Component({
  selector: 'app-mfa',
  standalone: true,
  imports: [ReactiveFormsModule, TextBoxComponent, RouterLink, TranslatePipe],
  templateUrl: './mfa.component.html',
  styleUrls: ['./mfa.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MfaComponent {
  private readonly fb = inject(FormBuilder);
  private readonly router = inject(Router);

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
      console.log('Successfully verified MFA code (mock).');
      this.router.navigate([`/${this.routePaths.DASHBOARD}`]);
    }, 1500);
  }

  resendCode(): void {
    console.log('Resending MFA code...');
    alert('A new verification code has been sent to your registered device.');
  }
}
