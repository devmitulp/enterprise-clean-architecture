import { OnInit, SHARED_ANGULAR_MODULES } from '@shared/angular';
import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { Select, InputGroup, InputGroupAddon, Tooltip } from '@primeng';
import { BaseControlValueAccessor } from '../base/base-control-value-accessor';
import { FormErrorComponent } from '../form-error/form-error.component';

@Component({
  selector: 'app-dropdown',
  standalone: true,
  imports: [
    ...SHARED_ANGULAR_MODULES,
    Select,
    InputGroup,
    InputGroupAddon,
    Tooltip,
    FormErrorComponent,
  ],
  templateUrl: './dropdown.component.html',
  styleUrl: './dropdown.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DropdownComponent<T = unknown> extends BaseControlValueAccessor<T> implements OnInit {
  @Input() icon = '';
  @Input({ required: true }) options: T[] = [];
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
