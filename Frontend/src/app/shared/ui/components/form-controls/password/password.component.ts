import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { Password } from 'primeng/password';
import { BaseControlValueAccessor } from '../base/base-control-value-accessor';

@Component({
  selector: 'app-password',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, Password, InputGroupModule, InputGroupAddonModule],
  templateUrl: './password.component.html',
  styleUrl: './password.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PasswordComponent extends BaseControlValueAccessor<string> implements OnInit {
  @Input() override id = 'password-' + crypto.randomUUID();
  @Input() override placeholder = '';
  @Input() icon = 'pi pi-lock';
  @Input() autoComplete = 'off';
  @Input() maxLength?: number;
  @Input() feedback = false;

  ngOnInit(): void {
    this.initialize();
  }

  onBlur(): void {
    this.blur();
  }
}
