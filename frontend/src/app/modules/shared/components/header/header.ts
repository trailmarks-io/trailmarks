import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { LanguageSwitcherComponent, TranslatePipe } from '../../../core';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, RouterLink, LanguageSwitcherComponent, TranslatePipe],
  templateUrl: './header.html'
})
export class HeaderComponent {
  isSideNavOpen = signal(false);

  toggleSideNav(): void {
    this.isSideNavOpen.update(value => !value);
  }

  closeSideNav(): void {
    this.isSideNavOpen.set(false);
  }
}
