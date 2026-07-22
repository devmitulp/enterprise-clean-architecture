import { ChangeDetectionStrategy, Component, computed, inject, input, output, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ROUTE_PATHS } from '@constants';
import { TranslatePipe } from '@ngx-translate/core';
import { LoggerService, ThemeService, LanguageService } from '@services';
import { AuthState, AuthService } from '@auth';

@Component({
  selector: 'app-topbar',
  standalone: true,
  imports: [RouterLink, TranslatePipe],
  templateUrl: './topbar.component.html',
  styleUrl: './topbar.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TopbarComponent {
  private readonly logger = inject(LoggerService);
  private readonly themeService = inject(ThemeService);
  private readonly languageService = inject(LanguageService);
  private readonly authState = inject(AuthState);
  private readonly authService = inject(AuthService);

  readonly routePaths = ROUTE_PATHS;

  // Inputs / Outputs
  readonly isSidebarCollapsed = input<boolean>(false);
  readonly toggleSidebar = output<void>();

  // State Signals
  readonly isDarkMode = signal(this.themeService.isDarkMode());
  readonly isProfileMenuOpen = signal(false);
  readonly isNotificationsOpen = signal(false);
  readonly isLanguageMenuOpen = signal(false);
  readonly currentLang = this.languageService.currentLang;
  readonly userContext = this.authState.userContext;

  readonly userInitials = computed(() => {
    const name = this.userContext()?.FullName;
    if (!name) return 'U';
    const parts = name.trim().split(/\s+/);
    if (parts.length >= 2) {
      return (parts[0].charAt(0) + parts[parts.length - 1].charAt(0)).toUpperCase();
    }
    return name.slice(0, 2).toUpperCase();
  });

  readonly availableLanguages = this.languageService.availableLanguages;

  toggleTheme(): void {
    const isDark = this.themeService.toggleTheme();
    this.isDarkMode.set(isDark);
  }

  toggleProfileMenu(): void {
    this.isProfileMenuOpen.update((val) => !val);
  }

  toggleNotifications(): void {
    this.isNotificationsOpen.update((val) => !val);
  }

  toggleLanguageMenu(): void {
    this.isLanguageMenuOpen.update((val) => !val);
  }

  changeLanguage(code: string): void {
    this.languageService.setLanguage(code);
    this.isLanguageMenuOpen.set(false);
  }

  logout(): void {
    this.logger.log('Logging out...');
    this.authService.logoutApi().subscribe({
      next: () => {
        this.authState.logout();
      },
      error: (err) => {
        this.logger.error('Logout API call failed', err);
        this.authState.logout();
      },
    });
  }
}
