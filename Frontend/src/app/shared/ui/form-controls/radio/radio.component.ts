import { OnInit, SHARED_ANGULAR_MODULES } from '@shared/angular';
import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { RadioButton, Tooltip } from '@primeng';
import { BaseControlValueAccessor } from '../base/base-control-value-accessor';
import { FormErrorComponent } from '../form-error/form-error.component';

@Component({
  selector: 'app-radio',
  standalone: true,
  imports: [...SHARED_ANGULAR_MODULES, RadioButton, Tooltip, FormErrorComponent],
  templateUrl: './radio.component.html',
  styleUrl: './radio.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RadioComponent<T> extends BaseControlValueAccessor<T> implements OnInit {
  @Input({ required: true }) options: T[] = [];
  @Input({ required: true }) name = '';
  @Input() optionLabel?: string;
  @Input() optionValue?: string;

  ngOnInit(): void {
    this.initialize();
  }

  onBlur(): void {
    this.blur();
  }

  getOptionLabel(option: T): string {
    if (this.optionLabel && option !== null && typeof option === 'object') {
      const value = (option as Record<PropertyKey, unknown>)[this.optionLabel];
      return value != null ? String(value) : '';
    }

    return String(option);
  }

  getOptionValue(option: T): any {
    if (this.optionValue && option !== null && typeof option === 'object') {
      return (option as Record<PropertyKey, unknown>)[this.optionValue];
    }
    return option;
  }
}
