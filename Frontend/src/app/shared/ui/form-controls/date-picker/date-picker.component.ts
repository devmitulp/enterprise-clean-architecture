import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { DatePicker } from 'primeng/datepicker';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { Tooltip } from 'primeng/tooltip';
import { BaseControlValueAccessor } from '../base/base-control-value-accessor';
import { FormErrorComponent } from '../form-error/form-error.component';

@Component({
  selector: 'app-date-picker',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    DatePicker,
    InputGroupModule,
    InputGroupAddonModule,
    Tooltip,
    FormErrorComponent,
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
