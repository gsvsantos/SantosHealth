import { DoctorDataPayload } from './doctor.models';
import { PatientDataPayload } from './patient.models';

export type ApiResponseDto =
  | { sucesso: true; dados: ApiResponseDataPayload }
  | { sucesso: false; erro: string[] };

export type ApiResponseDataPayload = PatientDataPayload | DoctorDataPayload | IdApiResponse;

export interface IdApiResponse {
  id: string;
}
