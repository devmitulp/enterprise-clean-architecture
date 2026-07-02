import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { ROUTE_PATHS } from '@constants';
import { TranslatePipe, TranslateService } from '@ngx-translate/core';
import { LoggerService } from '@services';
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
  styleUrls: ['./private-layout.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PrivateLayoutComponent {
  private translate = inject(TranslateService);
  private readonly logger = inject(LoggerService);
  readonly routePaths = ROUTE_PATHS;

  // Layout State Signals
  readonly isSidebarCollapsed = signal(false);
  readonly isDarkMode = signal(false);
  readonly isProfileMenuOpen = signal(false);
  readonly isNotificationsOpen = signal(false);
  readonly isLanguageMenuOpen = signal(false);
  readonly currentLang = signal(this.translate.currentLang.toString() || 'en');

  readonly availableLanguages = [
    { code: 'en', name: 'English' },
    { code: 'gu', name: 'ગુજરાતી' },
  ];

  constructor() {}

  // Sample User Permissions (would normally come from AuthFacade)
  readonly userPermissions = signal<string[]>(['read:dashboard', 'read:settings']);

  // Dynamic Navigation menu with role/permission based rendering
  readonly menuItems = signal<NavItem[]>([
    {
      label: 'LAYOUT.DASHBOARD',
      icon: 'pi pi-th-large',
      route: `/${ROUTE_PATHS.DASHBOARD}`,
      requiredPermission: 'read:dashboard',
    },
    {
      label: 'LAYOUT.SETTINGS',
      icon: 'pi pi-cog',
      route: `/${ROUTE_PATHS.SETTINGS}`,
      requiredPermission: 'read:settings',
    },
  ]);

  // Filtered menu based on user permissions
  readonly authorizedMenuItems = computed(() => {
    const permissions = this.userPermissions();
    return this.menuItems().filter(
      (item) => !item.requiredPermission || permissions.includes(item.requiredPermission),
    );
  });

  toggleSidebar(): void {
    this.isSidebarCollapsed.update((val) => !val);
  }

  toggleTheme(): void {
    this.isDarkMode.update((val) => !val);
    const htmlElement = document.documentElement;
    if (this.isDarkMode()) {
      htmlElement.classList.add('dark');
    } else {
      htmlElement.classList.remove('dark');
    }
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
    this.currentLang.set(code);
    this.translate.use(code);
    this.isLanguageMenuOpen.set(false);
  }

  logout(): void {
    // Placeholder logout implementation
    this.logger.log('Logging out...');
  }
}
