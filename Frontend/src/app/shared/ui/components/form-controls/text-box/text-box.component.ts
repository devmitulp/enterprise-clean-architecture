import { CommonModule } from '@angular/common';
import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  inject,
  Input,
  OnInit,
  Optional,
  Self,
} from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl, ReactiveFormsModule } from '@angular/forms';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { InputTextModule } from 'primeng/inputtext';
import { FormErrorComponent } from '../form-error/form-error.component';

@Component({
  selector: 'app-text-box',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    InputTextModule,
    FormErrorComponent,
    InputGroupModule,
    InputGroupAddonModule,
  ],
  templateUrl: './text-box.component.html',
  styleUrl: './text-box.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TextBoxComponent implements OnInit, ControlValueAccessor {
  @Input() id = 'input-' + crypto.randomUUID();
  @Input() label = '';
  @Input() placeholder = '';
  @Input() icon = '';
  @Input() required = false;
  @Input() readonly = false;
  @Input() autoComplete = 'on';
  @Input() maxLength?: number;

  disabled = false;

  // Single source of truth
  control!: FormControl;

  constructor(
    @Optional()
    @Self()
    public ngControl: NgControl,
  ) {
    if (this.ngControl) {
      this.ngControl.valueAccessor = this;
      this.control = new FormControl();
    }
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
