import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { ROUTE_PATHS } from '@constants';

@Component({
  selector: 'app-public-layout',
  standalone: true,
  imports: [
    RouterOutlet,
    RouterLink,
    RouterLinkActive
  ],
  templateUrl: './public-layout.component.html',
  styleUrl: './public-layout.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PublicLayoutComponent {
  readonly routePaths = ROUTE_PATHS;
}
