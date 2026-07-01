import { OnInit, SHARED_ANGULAR_MODULES } from '@shared/angular';
import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { PRIMENG_FEEDBACK_MODULES, PRIMENG_FORM_MODULES } from '@primeng';
import { BaseControlValueAccessor } from '../base/base-control-value-accessor';
import { FormErrorComponent } from '../form-error/form-error.component';

@Component({
  selector: 'app-number',
  standalone: true,
  imports: [
    ...SHARED_ANGULAR_MODULES,
    ...PRIMENG_FORM_MODULES,
    ...PRIMENG_FEEDBACK_MODULES,
    FormErrorComponent
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
