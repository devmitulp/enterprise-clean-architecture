import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { LoaderService } from '../../../../../core/services/loader.service';

@Component({
  selector: 'app-loader',
  standalone: true,
  templateUrl: './loader.component.html',
  styleUrl: './loader.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LoaderComponent {
  private readonly loaderService = inject(LoaderService);
  protected readonly isLoading = this.loaderService.isLoading;
}
