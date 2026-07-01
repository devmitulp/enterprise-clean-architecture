import { OnInit, SHARED_ANGULAR_MODULES } from '@shared/angular';
import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { FileUpload, Tooltip } from '@primeng';
import { BaseControlValueAccessor } from '../base/base-control-value-accessor';
import { FormErrorComponent } from '../form-error/form-error.component';

@Component({
  selector: 'app-file-upload',
  standalone: true,
  imports: [
    ...SHARED_ANGULAR_MODULES,
    FileUpload,
    Tooltip,
    FormErrorComponent
  ],
  templateUrl: './file-upload.component.html',
  styleUrl: './file-upload.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FileUploadComponent extends BaseControlValueAccessor<any> implements OnInit {
  @Input() multiple = false;
  @Input() accept = '';
  @Input() maxFileSize?: number;
  @Input() mode: 'basic' | 'advanced' = 'basic';
  @Input() auto = true;

  ngOnInit(): void {
    this.initialize();
  }

  onSelect(event: any): void {
    const files = event.files;
    if (this.multiple) {
      this.control.setValue(files);
    } else {
      this.control.setValue(files[0] || null);
    }
    this.onChange(this.control.value);
    this.onTouched();
  }

  onRemove(event: any): void {
    const removedFile = event.file;
    const currentValue = this.control.value;
    if (this.multiple && Array.isArray(currentValue)) {
      const remaining = currentValue.filter((f) => f !== removedFile);
      this.control.setValue(remaining.length > 0 ? remaining : null);
    } else {
      this.control.setValue(null);
    }
    this.onChange(this.control.value);
    this.onTouched();
  }

  onClear(): void {
    this.control.setValue(null);
    this.onChange(null);
    this.onTouched();
  }
}
