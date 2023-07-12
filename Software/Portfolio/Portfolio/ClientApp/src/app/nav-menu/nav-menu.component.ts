import { Component } from '@angular/core';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent {
  public isExpanded = false;

  /// Toggles whether the navbar is expanded or not on mobile devices
  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
