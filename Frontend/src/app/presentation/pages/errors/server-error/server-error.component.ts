import { Component, ChangeDetectionStrategy } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ROUTE_PATHS } from '../../../../core/constants/routes.constants';

@Component({
  selector: 'app-server-error',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './server-error.component.html',
  styleUrl: './server-error.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ServerErrorComponent {
  readonly routePaths = ROUTE_PATHS;
}
