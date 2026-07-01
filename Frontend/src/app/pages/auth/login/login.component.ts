import { ChangeDetectionStrategy, Component } from '@angular/core';
import { FormBuilder, FormGroup, Router, SHARED_ANGULAR_MODULES, Validators, inject, signal } from '@shared/angular';
import { TranslatePipe } from '@ngx-translate/core';
import { ROUTE_PATHS } from '@constants';
import { PasswordComponent } from '@form-controls';
import { TextBoxComponent } from '@form-controls';

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

  readonly routePaths = ROUTE_PATHS;
  readonly isSubmitting = signal(false);
  readonly loginError = signal<string | null>(null);

  // Strongly-typed reactive form
  readonly loginForm: FormGroup = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
    rememberMe: [false],
  });

  get emailControl() {
    return this.loginForm.get('email');
  }
  get passwordControl() {
    return this.loginForm.get('password');
  }

  onSubmit(): void {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    this.loginError.set(null);

    // Simulate authentication API delay
    setTimeout(() => {
      this.isSubmitting.set(false);
      console.log('Successfully logged in (mock).');
    }, 1500);
  }

  loginWithGoogle(): void {
    console.log('Initiating Google Login...');
  }

  loginWithMicrosoft(): void {
    console.log('Initiating Microsoft Login...');
  }
}
