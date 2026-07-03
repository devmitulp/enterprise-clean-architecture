import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { ROUTE_PATHS } from '@constants';
import { TranslatePipe } from '@ngx-translate/core';
import { LoggerService, ThemeService, LanguageService } from '@services';
import { AuthState } from '@auth';
import { computed, inject, signal } from '@shared/angular';

interface NavItem {
  label: string;
  icon: string;
  route: string;
  requiredPermission?: string;
}

@Component({
  selector: 'app-private-layout',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive, TranslatePipe],
  templateUrl: './private-layout.component.html',
  styleUrl: './private-layout.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PrivateLayoutComponent {
  private readonly logger = inject(LoggerService);
  private readonly themeService = inject(ThemeService);
  private readonly languageService = inject(LanguageService);
  private readonly authState = inject(AuthState);
  readonly routePaths = ROUTE_PATHS;

  // Layout State Signals
  readonly isSidebarCollapsed = signal(false);
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

  readonly availableLanguages = [
    { code: 'en', name: 'English' },
    { code: 'gu', name: 'ગુજરાતી' },
  ];

  constructor() {}

  // Dynamic Navigation menu (to be replaced with API-driven dynamic menu later)
  readonly menuItems = signal<NavItem[]>([
    {
      label: 'Dashboard',
      icon: 'pi pi-th-large',
      route: `/${ROUTE_PATHS.DASHBOARD}`,
    },
    {
      label: 'Settings',
      icon: 'pi pi-cog',
      route: `/${ROUTE_PATHS.SETTINGS}`,
    },
  ]);

  // Expose menu items to template
  readonly authorizedMenuItems = computed(() => this.menuItems());

  toggleSidebar(): void {
    this.isSidebarCollapsed.update((val) => !val);
  }

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
    this.authState.logout();
  }
}
