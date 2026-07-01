import { Component, ChangeDetectionStrategy, signal, computed, inject } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { TranslateService, TranslatePipe } from '@ngx-translate/core';
import { ROUTE_PATHS } from '@constants';

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
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PrivateLayoutComponent {
  private translate = inject(TranslateService);
  readonly routePaths = ROUTE_PATHS;

  // Layout State Signals
  readonly isSidebarCollapsed = signal(false);
  readonly isDarkMode = signal(false);
  readonly isProfileMenuOpen = signal(false);
  readonly isNotificationsOpen = signal(false);
  readonly isLanguageMenuOpen = signal(false);
  readonly currentLang = signal(localStorage.getItem('lang') || 'en');

  readonly availableLanguages = [
    { code: 'en', name: 'English' },
    { code: 'gu', name: 'ગુજરાતી' }
  ];

  constructor() {
    this.translate.setFallbackLang('en');
    this.translate.use(this.currentLang());
  }

  // Sample User Permissions (would normally come from AuthFacade)
  readonly userPermissions = signal<string[]>(['read:dashboard', 'read:settings']);

  // Dynamic Navigation menu with role/permission based rendering
  readonly menuItems = signal<NavItem[]>([
    {
      label: 'LAYOUT.DASHBOARD',
      icon: 'pi pi-th-large',
      route: `/${ROUTE_PATHS.DASHBOARD}`,
      requiredPermission: 'read:dashboard'
    },
    {
      label: 'LAYOUT.SETTINGS',
      icon: 'pi pi-cog',
      route: `/${ROUTE_PATHS.SETTINGS}`,
      requiredPermission: 'read:settings'
    }
  ]);

  // Filtered menu based on user permissions
  readonly authorizedMenuItems = computed(() => {
    const permissions = this.userPermissions();
    return this.menuItems().filter(item => 
      !item.requiredPermission || permissions.includes(item.requiredPermission)
    );
  });

  toggleSidebar(): void {
    this.isSidebarCollapsed.update(val => !val);
  }

  toggleTheme(): void {
    this.isDarkMode.update(val => !val);
    const htmlElement = document.documentElement;
    if (this.isDarkMode()) {
      htmlElement.classList.add('dark');
    } else {
      htmlElement.classList.remove('dark');
    }
  }

  toggleProfileMenu(): void {
    this.isProfileMenuOpen.update(val => !val);
  }

  toggleNotifications(): void {
    this.isNotificationsOpen.update(val => !val);
  }

  toggleLanguageMenu(): void {
    this.isLanguageMenuOpen.update(val => !val);
  }

  changeLanguage(code: string): void {
    this.currentLang.set(code);
    this.translate.use(code);
    localStorage.setItem('lang', code);
    this.isLanguageMenuOpen.set(false);
  }

  logout(): void {
    // Placeholder logout implementation
    console.log('Logging out...');
  }
}
