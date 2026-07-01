import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { Select } from 'primeng/select';
import { Tooltip } from 'primeng/tooltip';
import { BaseControlValueAccessor } from '../base/base-control-value-accessor';
import { FormErrorComponent } from '../form-error/form-error.component';

@Component({
  selector: 'app-dropdown',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    Select,
    InputGroupModule,
    InputGroupAddonModule,
    Tooltip,
    FormErrorComponent,
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
