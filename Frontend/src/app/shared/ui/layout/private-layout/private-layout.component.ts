import { ChangeDetectionStrategy, Component, computed, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ROUTE_PATHS } from '@constants';
import { NavItem, SidebarComponent, TopbarComponent, FooterComponent } from './components';

@Component({
  selector: 'app-private-layout',
  standalone: true,
  imports: [RouterOutlet, SidebarComponent, TopbarComponent, FooterComponent],
  templateUrl: './private-layout.component.html',
  styleUrl: './private-layout.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PrivateLayoutComponent {
  readonly routePaths = ROUTE_PATHS;

  // Default to collapsed on small screens (< 768px)
  readonly isSidebarCollapsed = signal(
    typeof window !== 'undefined' ? window.innerWidth < 768 : false
  );

  // Dynamic Navigation menu
  readonly menuItems = signal<NavItem[]>([
    {
      label: 'Dashboard',
      icon: 'fa-solid fa-gauge-high',
      route: `/${ROUTE_PATHS.DASHBOARD}`,
    },
    {
      label: 'Settings',
      icon: 'fa-solid fa-cog',
      route: `/${ROUTE_PATHS.SETTINGS}`,
    },
  ]);

  readonly authorizedMenuItems = computed(() => this.menuItems());

  toggleSidebar(): void {
    this.isSidebarCollapsed.update((val) => !val);
  }
}
