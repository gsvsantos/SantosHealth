import {
  ApplicationConfig,
  provideBrowserGlobalErrorListeners,
  provideZonelessChangeDetection,
  isDevMode,
} from '@angular/core';
import { provideNotification } from './providers/notificacao.provider';
import { provideRouter } from '@angular/router';

import { routes } from './routes/app.routes';
import { provideAuth } from './providers/auth.provider';
import { providePTBRDateFormat } from './providers/date-format.provider';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { TranslocoHttpLoader } from './transloco-loader';
import { provideTransloco } from '@jsverse/transloco';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZonelessChangeDetection(),
    provideTransloco({
      config: {
        availableLangs: ['en-US'],
        defaultLang: 'en-US',
        fallbackLang: 'en-US',
        reRenderOnLangChange: true,
        prodMode: !isDevMode(),
      },
      loader: TranslocoHttpLoader,
    }),
    provideRouter(routes),
    provideNotification(),
    provideAuth(),
    providePTBRDateFormat(),
    provideAnimationsAsync(),
  ],
};
