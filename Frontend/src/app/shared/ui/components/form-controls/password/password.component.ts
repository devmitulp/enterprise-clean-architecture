import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input, OnInit, Optional, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl, ReactiveFormsModule } from '@angular/forms';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { Password } from 'primeng/password';
import { FormErrorComponent } from '../form-error/form-error.component';

@Component({
  selector: 'app-password',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    Password,
    FormErrorComponent,
    InputGroupModule,
    InputGroupAddonModule,
  ],
  templateUrl: './password.component.html',
  styleUrl: './password.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PasswordComponent implements OnInit, ControlValueAccessor {
  @Input() id = 'password-' + crypto.randomUUID();
  @Input() label = '';
  @Input() placeholder = '';
  @Input() icon = 'pi pi-lock';
  @Input() required = false;
  @Input() readonly = false;
  @Input() autoComplete = 'off';
  @Input() maxLength?: number;
  @Input() feedback = false;

  disabled = false;

  // Single source of truth
  control = new FormControl('');

  constructor(
    @Optional()
    @Self()
    public ngControl: NgControl,
  ) {
    if (this.ngControl) {
      this.ngControl.valueAccessor = this;
    }
  }

  get displayControl(): FormControl {
    return (this.ngControl?.control as FormControl) ?? this.control;
  }

  ngOnInit(): void {
    if (this.ngControl?.control) {
      this.control.setValidators(this.ngControl.control.validator);
      this.control.setAsyncValidators(this.ngControl.control.asyncValidator);
      this.control.updateValueAndValidity({ emitEvent: false });
    }

    this.control.valueChanges.subscribe((value) => {
      this.onChange(value ?? '');
    });
  }

  // -------------------------
  // ControlValueAccessor
  // -------------------------

  private onChange: (value: string) => void = () => {};
  private onTouched: () => void = () => {};

  writeValue(value: string): void {
    this.control.setValue(value ?? '', {
      emitEvent: false,
    });
  }

  registerOnChange(fn: (value: string) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(disabled: boolean): void {
    this.disabled = disabled;

    if (disabled) {
      this.control.disable({ emitEvent: false });
    } else {
      this.control.enable({ emitEvent: false });
    }
  }

  onBlur(): void {
    this.onTouched();
  }
}
