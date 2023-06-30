import { Component, OnInit, Renderer2 } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'app';
  public theme: 'light' | 'dark' = 'light';

  constructor() {}

  /// Toggles between light and dark mode theme
  toggleTheme() {
    this.theme = (this.theme === 'light') ? 'dark' : 'light';
  }
}
