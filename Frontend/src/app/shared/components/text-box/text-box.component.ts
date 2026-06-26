import { Component, Input, forwardRef, signal, ChangeDetectionStrategy } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-text-box',
  standalone: true,
  templateUrl: './text-box.component.html',
  styleUrl: './text-box.component.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => TextBoxComponent),
      multi: true
    }
  ],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class TextBoxComponent implements ControlValueAccessor {
  @Input() id = 'input-' + Math.random().toString(36).substr(2, 9);
  @Input() label = '';
  @Input() type: 'text' | 'password' | 'email' | 'number' = 'text';
  @Input() placeholder = '';
  @Input() icon = '';
  @Input() hint = '';
  @Input() required = false;
  @Input() loading = false;
  @Input() readonly = false;
  @Input() invalid = false;

  readonly value = signal<string>('');
  disabled = false;

  // ControlValueAccessor Interface
  onChange: (value: string) => void = () => {};
  onTouched: () => void = () => {};

  writeValue(value: string): void {
    this.value.set(value || '');
  }

  registerOnChange(fn: (value: string) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  // Input events
  onInput(event: Event): void {
    const val = (event.target as HTMLInputElement).value;
    this.value.set(val);
    this.onChange(val);
  }

  onBlur(): void {
    this.onTouched();
  }
}
