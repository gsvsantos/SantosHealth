import { inject } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { Doctor } from '../models/doctor.models';
import { DoctorService } from '../services/doctor.service';

export const listDoctorsResolver: ResolveFn<Doctor[]> = () => {
  const doctorService = inject(DoctorService);
  return doctorService.getAll();
};
