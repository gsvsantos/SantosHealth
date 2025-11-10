import { EnvironmentProviders, makeEnvironmentProviders } from '@angular/core';
import {
  OWL_DATE_TIME_LOCALE,
  OWL_DATE_TIME_FORMATS,
} from '@danielmoncada/angular-datetime-picker';

export const BR_DATE_FORMATS = {
  parse: {
    dateInput: 'DD/MM/YYYY',
  },
  display: {
    dateInput: 'DD/MM/YYYY',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};

export const providePTBRDateFormat = (): EnvironmentProviders => {
  return makeEnvironmentProviders([
    { provide: OWL_DATE_TIME_LOCALE, useValue: 'pt-BR' },
    { provide: OWL_DATE_TIME_FORMATS, useValue: BR_DATE_FORMATS },
  ]);
};
