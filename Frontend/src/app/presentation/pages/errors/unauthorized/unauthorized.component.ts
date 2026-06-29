import { Component, ChangeDetectionStrategy } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ROUTE_PATHS } from '../../../../core/constants/routes.constants';

@Component({
  selector: 'app-unauthorized',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './unauthorized.component.html',
  styleUrl: './unauthorized.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UnauthorizedComponent {
  readonly routePaths = ROUTE_PATHS;
}
