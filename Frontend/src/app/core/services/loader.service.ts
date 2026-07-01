import { Injectable } from '@angular/core';
import { signal } from '@shared/angular';

@Injectable({
  providedIn: 'root',
})
export class LoaderService {
  private activeRequests = 0;
  private readonly _isLoading = signal(false);

  public readonly isLoading = this._isLoading.asReadonly();

  public show(): void {
    this.activeRequests++;
    this._isLoading.set(true);
  }

  public hide(): void {
    this.activeRequests--;
    if (this.activeRequests <= 0) {
      this.activeRequests = 0;
      this._isLoading.set(false);
    }
  }
}
