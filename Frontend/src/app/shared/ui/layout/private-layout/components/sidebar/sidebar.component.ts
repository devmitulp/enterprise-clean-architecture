import { ChangeDetectionStrategy, Component, input, output } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { TranslatePipe } from '@ngx-translate/core';

export interface NavItem {
  label: string;
  icon: string;
  route: string;
  requiredPermission?: string;
}

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, TranslatePipe],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SidebarComponent {
  readonly isCollapsed = input<boolean>(false);
  readonly menuItems = input<NavItem[]>([]);
  readonly toggleSidebar = output<void>();

  closeOnMobile(): void {
    if (typeof window !== 'undefined' && window.innerWidth < 768 && !this.isCollapsed()) {
      this.toggleSidebar.emit();
    }
  }
}
