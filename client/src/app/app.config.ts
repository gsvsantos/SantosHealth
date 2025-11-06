import {
  ApplicationConfig,
  provideBrowserGlobalErrorListeners,
  provideZonelessChangeDetection,
} from '@angular/core';
import { provideNotification } from './providers/notificacao.provider';
import { provideRouter } from '@angular/router';

import { routes } from './routes/app.routes';
import { provideAuth } from './providers/auth.provider';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZonelessChangeDetection(),
    provideRouter(routes),
    provideNotification(),
    provideAuth(),
  ],
};
