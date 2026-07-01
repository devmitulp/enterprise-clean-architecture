import { OnInit, SHARED_ANGULAR_MODULES } from '@shared/angular';
import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { InputText, InputGroup, InputGroupAddon, Tooltip } from '@primeng';
import { BaseControlValueAccessor } from '../base/base-control-value-accessor';
import { FormErrorComponent } from '../form-error/form-error.component';

@Component({
  selector: 'app-text-box',
  standalone: true,
  imports: [
    ...SHARED_ANGULAR_MODULES,
    InputText,
    InputGroup,
    InputGroupAddon,
    Tooltip,
    FormErrorComponent
  ],
  templateUrl: './text-box.component.html',
  styleUrl: './text-box.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TextBoxComponent extends BaseControlValueAccessor<string> implements OnInit {
  @Input() icon = '';

  ngOnInit(): void {
    this.initialize();
  }

  onBlur(): void {
    this.blur();
  }
}
