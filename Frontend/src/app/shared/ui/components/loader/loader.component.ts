import { ChangeDetectionStrategy, Component } from '@angular/core';
import { inject } from '@shared/angular';
import { LoaderService } from '@services';

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
