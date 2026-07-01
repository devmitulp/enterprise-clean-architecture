import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { RadioButton } from 'primeng/radiobutton';
import { Tooltip } from 'primeng/tooltip';
import { BaseControlValueAccessor } from '../base/base-control-value-accessor';
import { FormErrorComponent } from '../form-error/form-error.component';

@Component({
  selector: 'app-radio',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RadioButton, Tooltip, FormErrorComponent],
  templateUrl: './radio.component.html',
  styleUrl: './radio.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RadioComponent extends BaseControlValueAccessor<any> implements OnInit {
  @Input({ required: true }) options: any[] = [];
  @Input({ required: true }) name = '';
  @Input() optionLabel?: string;
  @Input() optionValue?: string;

  ngOnInit(): void {
    this.initialize();
  }

  onBlur(): void {
    this.blur();
  }

  getOptionLabel(option: any): string {
    if (this.optionLabel && option && typeof option === 'object') {
      return option[this.optionLabel];
    }
    return option?.label !== undefined ? option.label : String(option);
  }

  getOptionValue(option: any): any {
    if (this.optionValue && option && typeof option === 'object') {
      return option[this.optionValue];
    }
    return option?.value !== undefined ? option.value : option;
  }
}
