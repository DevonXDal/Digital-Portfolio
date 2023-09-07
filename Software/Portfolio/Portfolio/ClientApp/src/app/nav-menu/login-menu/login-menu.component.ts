import { DOCUMENT } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { Observable, map } from 'rxjs';

@Component({
  selector: 'app-login-menu',
  templateUrl: './login-menu.component.html',
  styleUrls: ['./login-menu.component.scss']
})
export class LoginMenuComponent implements OnInit {
  public isAuthenticated$?: Observable<boolean>;
  public userName$?: Observable<string | null | undefined>;

  constructor(private auth: AuthService, @Inject(DOCUMENT) private doc: Document) { }

  ngOnInit() {
    this.isAuthenticated$ = this.auth.isAuthenticated$;

    // Observe the state of who is logged in, if there is a user, then grab the name, otherwise put null
    this.userName$ = this.auth.user$.pipe(map(u => (u && u.name) || null));
  }

  public handleSignUp(): void {
    this.auth.loginWithRedirect({
      appState: {
        target: '/profile',
      },
      authorizationParams: {
        prompt: 'login',
        screen_hint: 'signup',
      },
    });
  }

  public handleLogin(): void {
    this.auth.loginWithRedirect({
      appState: {
        target: '/profile',
      },
      authorizationParams: {
        prompt: 'login',
      },
    });
  }

  public handleLogout(): void {
    this.auth.logout({
      logoutParams: {
        returnTo: this.doc.location.origin,
      },
    });
  }
}
