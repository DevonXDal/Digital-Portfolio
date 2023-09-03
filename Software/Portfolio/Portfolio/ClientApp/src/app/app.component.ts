import { style } from '@angular/animations';
import { ViewportScroller } from '@angular/common';
import { Component, OnInit, Renderer2 } from '@angular/core';
import { NavigationEnd, Router, RouterEvent } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import { filter } from 'rxjs';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  public env = environment;
  public shouldShowSummaryReturnLink = false;
  isAuth0Loading$ = this.auth.isLoading$;

  constructor(private router: Router, private auth: AuthService, private viewportScroller: ViewportScroller) {}

  /// Prepares the application by determining whether to use light mode or dark mode.
  ngOnInit(): void {
      let savedUiMode: string | null = localStorage.getItem('preferred-theme');
      this.router.events.subscribe((event) => {
        if (event instanceof NavigationEnd) {
          this.shouldShowSummaryReturnLink = !(this.router.url === '/');
        }
      })

      // Help from Medium article: https://pkief.medium.com/automatic-dark-mode-detection-in-angular-material-8342917885a0
      // Sees if the user has selected a theme, if they haven't, check if dark mode preference is set on their browser, 
      // finally default to light mode.
      if (savedUiMode && (savedUiMode === 'light' || savedUiMode === 'dark')) {
        this.env.uiMode = savedUiMode!;
      } else if (window.matchMedia && window.matchMedia("(prefers-color-scheme: dark").matches) {
        this.env.uiMode = 'dark';
      } else {
        this.env.uiMode = 'light';
      }
  }

  /// Toggles between light and dark mode theme and saves the current option
  toggleTheme() {
    this.env.uiMode = (this.env.uiMode === 'light') ? 'dark' : 'light';

    localStorage.setItem('preferred-theme', this.env.uiMode); // May throw error to console if storage is disabled
  }

  scrollToTop() {
    this.viewportScroller.scrollToPosition([0, 0]);
  }
}
