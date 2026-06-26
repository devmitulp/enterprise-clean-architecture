import { Component, ChangeDetectionStrategy } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ROUTE_PATHS } from '../../../core/constants/routes.constants';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [RouterLink],
  template: `
    <div class="flex min-h-[calc(100vh-4rem)] flex-col justify-center px-6 py-12 lg:px-8 bg-slate-50 dark:bg-slate-950">
      <div class="sm:mx-auto sm:w-full sm:max-w-sm">
        <h2 class="mt-10 text-center text-2xl font-bold leading-9 tracking-tight text-slate-900 dark:text-white">Reset Password</h2>
      </div>
      <div class="mt-10 sm:mx-auto sm:w-full sm:max-w-sm">
        <div class="text-center">
          <a [routerLink]="[routePaths.LOGIN]" class="font-semibold text-indigo-600 dark:text-indigo-400 hover:text-indigo-500">Back to Login</a>
        </div>
      </div>
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ResetPasswordComponent {
  readonly routePaths = ROUTE_PATHS;
}
