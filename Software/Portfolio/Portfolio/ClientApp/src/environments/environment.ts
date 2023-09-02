// This file can be replaced during build by using the `fileReplacements` array.
// `ng build` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  uiMode: 'light',
  auth0: {
    domain: 'dev-llzpcvhdu4afegp2.us.auth0.com',
    clientId: 'P0w90qzKWM1ctKDgPegI771hVCxBU4Gf',
    authorizationParams: {
      audience: 'https://hello-world.example.com',
      redirect_uri: 'https://localhost:4000/callback',
    },
    errorPath: '/callback',
  },
  api: {
    serverUrl: 'http://localhost:8080',
  },
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/plugins/zone-error';  // Included with Angular CLI.
