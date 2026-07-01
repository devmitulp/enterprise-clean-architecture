import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { InputTextModule } from 'primeng/inputtext';
import { Tooltip } from 'primeng/tooltip';
import { BaseControlValueAccessor } from '../base/base-control-value-accessor';
import { FormErrorComponent } from '../form-error/form-error.component';

@Component({
  selector: 'app-text-box',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    InputTextModule,
    FormErrorComponent,
    InputGroupModule,
    InputGroupAddonModule,
    Tooltip,
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
