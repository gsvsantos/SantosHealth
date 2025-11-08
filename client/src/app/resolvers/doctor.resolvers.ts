import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, ResolveFn } from '@angular/router';
import { Doctor } from '../models/doctor.models';
import { DoctorService } from '../services/doctor.service';

export const listDoctorsResolver: ResolveFn<Doctor[]> = () => {
  const doctorService = inject(DoctorService);
  return doctorService.getAll();
};

export const doctorDetailsResolver: ResolveFn<Doctor> = (route: ActivatedRouteSnapshot) => {
  const doctorService = inject(DoctorService);

  if (!route.paramMap.has('id')) throw new Error('Missing "ID" parameter.');

  const id: string = route.paramMap.get('id')!;

  return doctorService.getById(id);
};
