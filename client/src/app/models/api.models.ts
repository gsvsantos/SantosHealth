import { DoctorDataPayload } from './doctor.models';
import { PatientDataPayload } from './patient.models';

export type ApiResponseDto =
  | { sucesso: true; dados: PatientDataPayload | DoctorDataPayload }
  | { sucesso: false; erro: string[] };
