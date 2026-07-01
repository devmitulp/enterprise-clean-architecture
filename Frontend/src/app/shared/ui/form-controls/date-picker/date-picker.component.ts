import { OnInit, SHARED_ANGULAR_MODULES } from '@shared/angular';
import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { DatePicker, InputGroup, InputGroupAddon, Tooltip } from '@primeng';
import { BaseControlValueAccessor } from '../base/base-control-value-accessor';
import { FormErrorComponent } from '../form-error/form-error.component';

@Component({
  selector: 'app-date-picker',
  standalone: true,
  imports: [
    ...SHARED_ANGULAR_MODULES,
    DatePicker,
    InputGroup,
    InputGroupAddon,
    Tooltip,
    FormErrorComponent
  ],
  templateUrl: './date-picker.component.html',
  styleUrl: './date-picker.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DatePickerComponent extends BaseControlValueAccessor<Date | Date[]> implements OnInit {
  @Input() icon = '';
  @Input() showIcon = true;
  @Input() selectionMode: 'single' | 'multiple' | 'range' = 'single';
  @Input() dateFormat = 'dd/mm/yy';
  @Input() showTime = false;
  @Input() hourFormat: '12' | '24' = '24';
  @Input() minDate?: Date;
  @Input() maxDate?: Date;

  ngOnInit(): void {
    this.initialize();
  }

  onBlur(): void {
    this.blur();
  }
}
