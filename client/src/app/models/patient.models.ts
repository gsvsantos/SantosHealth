import { Doctor } from './doctor.models';

export interface PatientDto {
  nome: string;
  cpf: string;
  email: string;
  telefone: string;
}

export interface IdApiResponse {
  id: string;
}

export interface ListPatientsDto {
  quantidadeRegistros: number;
  registros: Patient[];
}

export interface Patient extends PatientDto {
  id: string;
}

export interface PatientDetailsDto extends Patient {
  atividades: Activity[];
}

export interface Activity {
  id: string;
  inicio: Date;
  termino: Date;
  tipoAtividade: string;
  medicos: Doctor[];
}

export type PatientDataPayload = ListPatientsDto | IdApiResponse | PatientDetailsDto;
