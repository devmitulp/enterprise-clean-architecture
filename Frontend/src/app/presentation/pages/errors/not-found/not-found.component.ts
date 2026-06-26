import { Component, ChangeDetectionStrategy } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ROUTE_PATHS } from '../../../../core/constants/routes.constants';

@Component({
  selector: 'app-not-found',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './not-found.component.html',
  styleUrl: './not-found.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class NotFoundComponent {
  readonly routePaths = ROUTE_PATHS;
}
