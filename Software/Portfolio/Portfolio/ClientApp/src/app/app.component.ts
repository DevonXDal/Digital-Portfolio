import { style } from '@angular/animations';
import { Component, OnInit, Renderer2 } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'app';
  public uiMode: 'light' | 'dark' = 'light';

  constructor() {}

  /// Prepares the application by determining whether to use light mode or dark mode.
  ngOnInit(): void {
      let savedUiMode: string | null = localStorage.getItem('preferred-theme');

      // Help from Medium article: https://pkief.medium.com/automatic-dark-mode-detection-in-angular-material-8342917885a0
      // Sees if the user has selected a theme, if they haven't, check if dark mode preference is set on their browser, 
      // finally default to light mode.
      if (savedUiMode && (savedUiMode === 'light' || savedUiMode === 'dark')) {
        this.uiMode = savedUiMode!;
      } else if (window.matchMedia && window.matchMedia("(prefers-color-scheme: dark").matches) {
        this.uiMode = 'dark';
      } else {
        this.uiMode = 'light';
      }
  }

  /// Toggles between light and dark mode theme and saves the current option
  toggleTheme() {
    this.uiMode = (this.uiMode === 'light') ? 'dark' : 'light';

    localStorage.setItem('preferred-theme', this.uiMode); // May throw error to console if storage is disabled
  }
}
