import { EnvironmentProviders, makeEnvironmentProviders } from '@angular/core';
import {
  OWL_DATE_TIME_LOCALE,
  OWL_DATE_TIME_FORMATS,
} from '@danielmoncada/angular-datetime-picker';
import { LOCALE_ID } from '@angular/core';
import localePt from '@angular/common/locales/pt';
import { registerLocaleData } from '@angular/common';
import 'dayjs/locale/pt-br';
import dayjs from 'dayjs';

export const BR_DATE_FORMATS = {
  parse: {
    dateInput: 'MM/DD/YYYY',
  },
  display: {
    dateInput: 'DD/MM/YYYY',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};  

export const providePTBRDateFormat = (): EnvironmentProviders => {
  registerLocaleData(localePt);

  dayjs().locale('pt-br');

  return makeEnvironmentProviders([
    { provide: LOCALE_ID, useValue: 'pt-BR' },
    { provide: OWL_DATE_TIME_LOCALE, useValue: 'pt-BR' },
    { provide: OWL_DATE_TIME_FORMATS, useValue: BR_DATE_FORMATS },
  ]);
};
