import { Component, HostListener } from '@angular/core';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent {
  public isExpanded = false;
  
  constructor() {}

  /// Toggles whether the navbar is expanded or not on mobile devices.
  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  /// Listens for changes in screen size and sets isExpanded to false if the screen is a large or greater screen.
  @HostListener('window:resize', ['$event'])
  checkScreenSize() {
    if (window.innerWidth >= 1024) this.isExpanded = false;
  }
}
