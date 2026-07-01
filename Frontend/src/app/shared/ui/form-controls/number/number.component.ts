import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { InputNumber } from 'primeng/inputnumber';
import { Tooltip } from 'primeng/tooltip';
import { BaseControlValueAccessor } from '../base/base-control-value-accessor';
import { FormErrorComponent } from '../form-error/form-error.component';

@Component({
  selector: 'app-number',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    InputNumber,
    InputGroupModule,
    InputGroupAddonModule,
    Tooltip,
    FormErrorComponent,
  ],
  templateUrl: './number.component.html',
  styleUrl: './number.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NumberComponent extends BaseControlValueAccessor<number> implements OnInit {
  @Input() icon = '';
  @Input() min?: number;
  @Input() max?: number;
  @Input() useGrouping = true;
  @Input() minFractionDigits?: number;
  @Input() maxFractionDigits?: number;

  ngOnInit(): void {
    this.initialize();
  }

  onBlur(): void {
    this.blur();
  }
}
