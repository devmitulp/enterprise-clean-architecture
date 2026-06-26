import { Component, ChangeDetectionStrategy, signal, computed } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { ROUTE_PATHS } from '../../../core/constants/routes.constants';

interface NavItem {
  label: string;
  icon: string;
  route: string;
  requiredPermission?: string;
}

@Component({
  selector: 'app-private-layout',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './private-layout.component.html',
  styleUrls: ['./private-layout.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PrivateLayoutComponent {
  readonly routePaths = ROUTE_PATHS;

  // Layout State Signals
  readonly isSidebarCollapsed = signal(false);
  readonly isDarkMode = signal(false);
  readonly isProfileMenuOpen = signal(false);
  readonly isNotificationsOpen = signal(false);

  // Sample User Permissions (would normally come from AuthFacade)
  readonly userPermissions = signal<string[]>(['read:dashboard', 'read:settings']);

  // Dynamic Navigation menu with role/permission based rendering
  readonly menuItems = signal<NavItem[]>([
    {
      label: 'Dashboard',
      icon: 'pi pi-th-large',
      route: `/${ROUTE_PATHS.DASHBOARD}`,
      requiredPermission: 'read:dashboard'
    },
    {
      label: 'Settings',
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

  logout(): void {
    // Placeholder logout implementation
    console.log('Logging out...');
  }
}
