import { OnInit, SHARED_ANGULAR_MODULES } from '@shared/angular';
import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { Password, InputGroup, InputGroupAddon, Tooltip } from '@primeng';
import { BaseControlValueAccessor } from '../base/base-control-value-accessor';
import { FormErrorComponent } from '../form-error/form-error.component';

@Component({
  selector: 'app-password',
  standalone: true,
  imports: [
    ...SHARED_ANGULAR_MODULES,
    Password,
    InputGroup,
    InputGroupAddon,
    Tooltip,
    FormErrorComponent,
  ],
  templateUrl: './password.component.html',
  styleUrl: './password.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PasswordComponent extends BaseControlValueAccessor<string> implements OnInit {
  @Input() icon = 'fa-solid fa-lock';
  @Input() feedback = false;
  @Input() toggleMask = true;

  ngOnInit(): void {
    this.initialize();
  }

  onBlur(): void {
    this.blur();
  }
}
