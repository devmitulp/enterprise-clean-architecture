import { Directive, inject, Input, OnInit, DestroyRef } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Directive()
export abstract class BaseControlValueAccessor<T = any> implements ControlValueAccessor {
  // -------------------------------------------------------------------------
  // Common Inputs
  // -------------------------------------------------------------------------
  @Input() id = 'input-' + crypto.randomUUID();
  @Input() label = '';
  @Input() placeholder = '';
  @Input() required = false;
  @Input() readonly = false;

  disabled = false;
  /**
   * Internal control (Input Binding)
   */
  readonly control = new FormControl<T | null>(null);
  /**
   * Parent FormControl (Validation)
   */
  get displayControl(): FormControl {
    return this.ngControl?.control as FormControl;
  }

  protected readonly destroyRef = inject(DestroyRef);

  protected readonly ngControl = inject(NgControl, {
    optional: true,
    self: true,
  });

  constructor() {
    if (this.ngControl) {
      this.ngControl.valueAccessor = this;
    }
  }

  // ---------------------------
  // Init
  // ---------------------------

  protected initialize(): void {
    this.control.valueChanges.pipe(takeUntilDestroyed(this.destroyRef)).subscribe((value) => {
      this.onChange(value as T);
    });
  }

  // ---------------------------
  // CVA
  // ---------------------------

  protected onChange: (value: T) => void = () => {};
  protected onTouched: () => void = () => {};

  writeValue(value: T): void {
    this.control.setValue(value, {
      emitEvent: false,
    });
  }

  registerOnChange(fn: (value: T) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;

    if (isDisabled) {
      this.control.disable({
        emitEvent: false,
      });
    } else {
      this.control.enable({
        emitEvent: false,
      });
    }
  }

  protected blur(): void {
    this.onTouched();
  }
}
