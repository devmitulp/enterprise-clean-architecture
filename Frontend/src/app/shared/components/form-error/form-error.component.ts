import { Component, Input, ChangeDetectionStrategy } from '@angular/core';
import { AbstractControl } from '@angular/forms';

@Component({
  selector: 'app-form-error',
  standalone: true,
  templateUrl: './form-error.component.html',
  styleUrl: './form-error.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
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
