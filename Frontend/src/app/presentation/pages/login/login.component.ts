import { Component, ChangeDetectionStrategy, signal, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { ROUTE_PATHS } from '../../../core/constants/routes.constants';
import { TextBoxComponent } from '../../../shared/components/text-box/text-box.component';
import { FormErrorComponent } from '../../../shared/components/form-error/form-error.component';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    ReactiveFormsModule, 
    RouterLink, 
    TextBoxComponent, 
    FormErrorComponent
  ],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
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
    rememberMe: [false]
  });

  get emailControl() { return this.loginForm.get('email'); }
  get passwordControl() { return this.loginForm.get('password'); }

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
      
      // Perform mock navigation to dashboard on successful mock login
      this.router.navigate([`/${ROUTE_PATHS.DASHBOARD}`]);
    }, 1500);
  }

  loginWithGoogle(): void {
    console.log('Initiating Google Login...');
  }

  loginWithMicrosoft(): void {
    console.log('Initiating Microsoft Login...');
  }
}
