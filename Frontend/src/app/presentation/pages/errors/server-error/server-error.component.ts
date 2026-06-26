import { Component, ChangeDetectionStrategy } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ROUTE_PATHS } from '../../../../core/constants/routes.constants';

@Component({
  selector: 'app-server-error',
  standalone: true,
  imports: [RouterLink],
  template: `
    <div class="flex min-h-screen flex-col justify-center items-center px-6 py-12 bg-slate-50 dark:bg-slate-950">
      <div class="text-center">
        <p class="text-base font-semibold text-rose-600 dark:text-rose-400">500</p>
        <h1 class="mt-4 text-3xl font-bold tracking-tight text-slate-900 dark:text-white sm:text-5xl">Internal Server Error</h1>
        <p class="mt-6 text-base leading-7 text-slate-600 dark:text-slate-400">Something went wrong on our end. Please try again later.</p>
        <div class="mt-10 flex items-center justify-center gap-x-6">
          <a [routerLink]="[routePaths.HOME]" class="rounded-xl bg-indigo-600 px-3.5 py-2.5 text-sm font-semibold text-white shadow-sm hover:bg-indigo-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600">Go back home</a>
        </div>
      </div>
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ServerErrorComponent {
  readonly routePaths = ROUTE_PATHS;
}
