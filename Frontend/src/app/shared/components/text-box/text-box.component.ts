import { Component, Input, forwardRef, signal, ChangeDetectionStrategy } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-text-box',
  standalone: true,
  template: `
    <div class="w-full flex flex-col gap-1.5">
      <!-- Input Label -->
      @if (label) {
        <label [for]="id" class="text-xs font-semibold text-slate-700 dark:text-slate-300">
          {{ label }}
          @if (required) {
            <span class="text-rose-500 font-bold">*</span>
          }
        </label>
      }

      <!-- Input Field Container -->
      <div 
        [class.border-rose-500]="invalid"
        [class.ring-rose-500]="invalid"
        [class.opacity-60]="disabled"
        [class.pointer-events-none]="disabled"
        class="relative flex items-center w-full rounded-xl border border-slate-200 dark:border-slate-800 bg-white dark:bg-slate-900 shadow-sm focus-within:border-indigo-500 focus-within:ring-2 focus-within:ring-indigo-500/25 transition-all duration-200">
        
        <!-- Prefix Icon -->
        @if (icon) {
          <div class="pl-3.5 pr-2.5 text-slate-400 dark:text-slate-500 flex items-center justify-center">
            <i [class]="icon"></i>
          </div>
        }

        <!-- Native Input -->
        <input
          [id]="id"
          [type]="type"
          [value]="value()"
          [placeholder]="placeholder"
          [disabled]="disabled"
          [readOnly]="readonly"
          (input)="onInput($event)"
          (blur)="onBlur()"
          class="w-full py-2.5 px-3.5 text-sm bg-transparent rounded-xl focus:outline-none placeholder-slate-400 text-slate-900 dark:text-white"
        />

        <!-- Loading Spinner -->
        @if (loading) {
          <div class="pr-3.5 text-indigo-500 flex items-center justify-center">
            <i class="pi pi-spin pi-spinner text-sm animate-spin"></i>
          </div>
        }
      </div>

      <!-- Hint Text -->
      @if (hint && !invalid) {
        <p class="text-[11px] text-slate-500 dark:text-slate-400 leading-normal">{{ hint }}</p>
      }
    </div>
  `,
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
