import { BrowserModule } from '@angular/platform-browser';
import { NgModule, isDevMode } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { ServiceWorkerModule } from '@angular/service-worker';
import { PortfolioSummaryComponent } from './portfolio-summary/portfolio-summary.component';
import { ContactMeComponent } from './contact-me/contact-me.component';
import { LearnMoreModule } from './learn-more/learn-more.module';
import { OtherContentModule } from './other-content/other-content.module';
import { SharedModule } from './shared/shared.module';

import { TabViewModule } from "primeng/tabview";
import { AuthHttpInterceptor, AuthModule } from '@auth0/auth0-angular';

import { environment as env } from '../environments/environment';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    CounterComponent,
    FetchDataComponent,
    PortfolioSummaryComponent,
    ContactMeComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    AuthModule.forRoot({
      ...env.auth0,
      httpInterceptor: {
        allowedList: [],
      },
    }),
    RouterModule.forRoot([
      { path: '', component: PortfolioSummaryComponent, pathMatch: 'full' },
      { path: 'contact', component: ContactMeComponent },
      { path: 'counter', component: CounterComponent },
    ]),
    ServiceWorkerModule.register('ngsw-worker.js', {
      enabled: !isDevMode(),
      // Register the ServiceWorker as soon as the application is stable
      // or after 30 seconds (whichever comes first).
      registrationStrategy: 'registerWhenStable:30000'
    }),
    LearnMoreModule,
    OtherContentModule,
    TabViewModule,
    SharedModule,
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthHttpInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
