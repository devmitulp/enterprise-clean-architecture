import { OnInit, SHARED_ANGULAR_MODULES } from '@shared/angular';
import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { Checkbox, Tooltip } from '@primeng';
import { BaseControlValueAccessor } from '../base/base-control-value-accessor';
import { FormErrorComponent } from '../form-error/form-error.component';

@Component({
  selector: 'app-checkbox',
  standalone: true,
  imports: [
    ...SHARED_ANGULAR_MODULES,
    Checkbox,
    Tooltip,
    FormErrorComponent
  ],
  templateUrl: './checkbox.component.html',
  styleUrl: './checkbox.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CheckboxComponent extends BaseControlValueAccessor<boolean | any[]> implements OnInit {
  @Input() checkboxLabel = '';
  @Input() binary = true;
  @Input() value?: any;

  ngOnInit(): void {
    this.initialize();
  }

  onBlur(): void {
    this.blur();
  }
}
