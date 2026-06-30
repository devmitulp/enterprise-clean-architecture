import { Component, ChangeDetectionStrategy, signal, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ROUTE_PATHS } from '../../../../core/constants/routes.constants';
import { TextBoxComponent } from '../../../../shared/components/text-box/text-box.component';
import { TranslatePipe } from '@ngx-translate/core';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, TextBoxComponent, TranslatePipe],
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
