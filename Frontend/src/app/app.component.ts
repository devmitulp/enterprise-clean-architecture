import { Component } from '@angular/core';
import { signal } from '@shared/angular';
import { RouterOutlet } from '@angular/router';
import { LoaderComponent } from '@components';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    LoaderComponent
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  protected readonly title = signal('enterprise-frontend');
}
