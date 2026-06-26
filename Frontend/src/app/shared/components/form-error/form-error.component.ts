import { Component, Input, ChangeDetectionStrategy } from '@angular/core';
import { AbstractControl } from '@angular/forms';

@Component({
  selector: 'app-form-error',
  standalone: true,
  template: `
    @if (control && control.invalid && (control.dirty || control.touched)) {
      <div class="mt-1.5 text-xs text-rose-600 dark:text-rose-400 flex items-center gap-1.5 animate-fadeIn">
        <i class="pi pi-exclamation-circle text-xs"></i>
        <span>{{ errorMessage }}</span>
      </div>
    }
  `,
  styles: [`
    @keyframes fadeIn {
      from { opacity: 0; transform: translateY(-3px); }
      to { opacity: 1; transform: translateY(0); }
    }
    .animate-fadeIn {
      animation: fadeIn 200ms ease-out forwards;
    }
  `],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class FormErrorComponent {
  @Input({ required: true }) control!: AbstractControl | null;

  get errorMessage(): string {
    if (!this.control || !this.control.errors) return '';

    const errors = this.control.errors;

    if (errors['required']) return 'This field is required.';
    if (errors['email']) return 'Please enter a valid email address.';
    if (errors['minlength']) {
      return `Minimum length is ${errors['minlength'].requiredLength} characters.`;
    }
    if (errors['maxlength']) {
      return `Maximum length is ${errors['maxlength'].requiredLength} characters.`;
    }
    if (errors['pattern']) return 'Invalid format.';
    if (errors['customError']) return errors['customError'];

    return 'Invalid field value.';
  }
}
