import { EnvironmentProviders, makeEnvironmentProviders } from '@angular/core';
import { MAT_SNACK_BAR_DEFAULT_OPTIONS } from '@angular/material/snack-bar';
import { NotificacaoService } from '../services/notificacao.service';

export const provideNotification = (): EnvironmentProviders => {
  return makeEnvironmentProviders([
    {
      provide: MAT_SNACK_BAR_DEFAULT_OPTIONS,
      useValue: {
        duration: 33000,
        horizontalPosition: 'end',
        verticalPosition: 'bottom',
      },
    },

    NotificacaoService,
  ]);
};
