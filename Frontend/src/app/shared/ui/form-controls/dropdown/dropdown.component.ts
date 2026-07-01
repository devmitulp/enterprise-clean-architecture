import { OnInit, SHARED_ANGULAR_MODULES } from '@shared/angular';
import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { PRIMENG_FEEDBACK_MODULES, PRIMENG_FORM_MODULES } from '@primeng';
import { BaseControlValueAccessor } from '../base/base-control-value-accessor';
import { FormErrorComponent } from '../form-error/form-error.component';

@Component({
  selector: 'app-dropdown',
  standalone: true,
  imports: [
    ...SHARED_ANGULAR_MODULES,
    ...PRIMENG_FORM_MODULES,
    ...PRIMENG_FEEDBACK_MODULES,
    FormErrorComponent
  ],
  templateUrl: './dropdown.component.html',
  styleUrl: './dropdown.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DropdownComponent extends BaseControlValueAccessor<any> implements OnInit {
  @Input() icon = '';
  @Input({ required: true }) options: any[] = [];
  @Input() optionLabel?: string;
  @Input() optionValue?: string;
  @Input() filter = false;
  @Input() filterBy?: string;
  @Input() showClear = true;

  ngOnInit(): void {
    this.initialize();
  }

  onBlur(): void {
    this.blur();
  }
}
