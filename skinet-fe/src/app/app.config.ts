import { ApplicationConfig, inject, provideAppInitializer, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { errorInterceptor } from './core/interceptors/error-interceptor';
import { loadingInterceptor } from './core/interceptors/loading-interceptor';
import { InitService } from './core/services/init.service';
import { lastValueFrom } from 'rxjs';
import { authInterceptor } from './core/interceptors/auth-interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideHttpClient(
      withInterceptors([
        errorInterceptor,
        loadingInterceptor,
        authInterceptor
      ])),
      // Inside provideAppInitializer, if you give it a Observable/Promise, it will wait until the O/P function is resolve 
      //and then continue process the app (start other component OnInit)
    provideAppInitializer(async () => {
      const initService = inject(InitService);
      // Get cart and user info before the app starts 

      // initService.init() is observerble, when use lastValueFrom() => init() start processing and start process other
      // observables inside init() as well.
      return lastValueFrom(initService.init()).finally(() => {
        const splash = document.getElementById('initial-splash');
        if (splash) splash.remove();
      });
    })
  ]
};