import { OnInit, SHARED_ANGULAR_MODULES } from '@shared/angular';
import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { PRIMENG_FEEDBACK_MODULES, PRIMENG_FORM_MODULES } from '@primeng';
import { BaseControlValueAccessor } from '../base/base-control-value-accessor';
import { FormErrorComponent } from '../form-error/form-error.component';

@Component({
  selector: 'app-textarea',
  standalone: true,
  imports: [
    ...SHARED_ANGULAR_MODULES,
    ...PRIMENG_FORM_MODULES,
    ...PRIMENG_FEEDBACK_MODULES,
    FormErrorComponent
  ],
  templateUrl: './textarea.component.html',
  styleUrl: './textarea.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TextAreaComponent extends BaseControlValueAccessor<string> implements OnInit {
  @Input() rows = 3;
  @Input() cols?: number;
  @Input() autoResize = true;

  ngOnInit(): void {
    this.initialize();
  }

  onBlur(): void {
    this.blur();
  }
}
