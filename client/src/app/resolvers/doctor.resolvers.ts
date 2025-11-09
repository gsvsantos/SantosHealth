import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, ResolveFn } from '@angular/router';
import { Doctor, DoctorDtoToTop10 } from '../models/doctor.models';
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

export const listTop10DoctorsResolver: ResolveFn<DoctorDtoToTop10[]> = () => {
  const doctorService = inject(DoctorService);
  return doctorService.getTop10(null, null);
};
