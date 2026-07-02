import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { AbstractControl, ChangeDetectorRef, DestroyRef, inject, takeUntilDestroyed } from '@shared/angular';
import { LOCALIZATION_KEYS, LocalizerService } from '@shared/localization';

@Component({
  selector: 'app-form-error',
  standalone: true,
  templateUrl: './form-error.component.html',
  styleUrl: './form-error.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FormErrorComponent {
  private readonly cdr = inject(ChangeDetectorRef);
  private readonly destroyRef = inject(DestroyRef);
  private readonly localizer = inject(LocalizerService);
  private _control: AbstractControl | null = null;

  @Input() label = 'This field';
  @Input() id = '';
  @Input({ required: true })
  set control(value: AbstractControl | null) {
    if (!value || this._control === value) {
      return;
    }

    this._control = value;

    this._control.events.pipe(takeUntilDestroyed(this.destroyRef)).subscribe(() => {
      this.cdr.detectChanges();
    });
  }

  get control(): AbstractControl | null {
    return this._control;
  }

  get showError(): boolean {
    return !!this.control && this.control.invalid && (this.control.dirty || this.control.touched);
  }

  get errorMessage(): string {
    if (!this.control?.errors) {
      return '';
    }

    const errors = this.control.errors;
    const fieldName = this.label?.trim() || 'This field';

    const validationOrder = [
      'required',
      'email',
      'minlength',
      'maxlength',
      'min',
      'max',
      'pattern',
      'customError',
      'message',
    ];

    for (const key of validationOrder) {
      if (!errors[key]) {
        continue;
      }

      switch (key) {
        case 'required':
          return this.localizer.get(LOCALIZATION_KEYS.Required, fieldName);

        case 'email':
          return this.localizer.get(LOCALIZATION_KEYS.Email, fieldName);

        case 'minlength':
          return this.localizer.get(
            LOCALIZATION_KEYS.MinLength,
            fieldName,
            errors['minlength'].requiredLength,
          );

        case 'maxlength':
          return this.localizer.get(
            LOCALIZATION_KEYS.MaxLength,
            fieldName,
            errors['maxlength'].requiredLength,
          );

        case 'min':
          return this.localizer.get(LOCALIZATION_KEYS.Min, fieldName, errors['min'].min);

        case 'max':
          return this.localizer.get(LOCALIZATION_KEYS.Max, fieldName, errors['max'].max);

        case 'pattern':
          return this.localizer.get(LOCALIZATION_KEYS.Pattern, fieldName);

        case 'customError':
          return errors['customError'];

        case 'message':
          return errors['message'];
      }
    }

    // Generic custom validator fallback
    for (const key of Object.keys(errors)) {
      const value = errors[key];

      if (typeof value === 'string') {
        return value;
      }

      if (value?.message) {
        return value.message;
      }
    }

    return this.localizer.get('Invalid', fieldName);
  }
}
