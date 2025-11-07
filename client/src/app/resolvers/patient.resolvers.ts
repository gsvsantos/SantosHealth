import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, ResolveFn } from '@angular/router';
import { Patient, PatientDetailsDto } from '../models/patient.models';
import { PatientService } from '../services/patient.service';

export const listPatientsResolver: ResolveFn<Patient[]> = () => {
  const patientService = inject(PatientService);
  return patientService.getAll();
};

export const patientDetailsResolver: ResolveFn<PatientDetailsDto> = (
  route: ActivatedRouteSnapshot,
) => {
  const patientService = inject(PatientService);

  if (!route.paramMap.has('id')) throw new Error('Missing "ID" parameter.');

  const id: string = route.paramMap.get('id')!;

  return patientService.getById(id);
};
