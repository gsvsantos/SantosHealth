import { Doctor } from './doctor.models';
import { Patient } from './patient.models';

export interface ActivityDto {
  pacienteId: string;
  inicio: Date;
  termino: Date;
  tipoAtividade: string;
  medicos: string[];
}

export interface EditActivityDto {
  inicio: Date;
  termino: Date;
  medicos: string[];
}

export interface ListActivitiesDto {
  quantidadeRegistros: number;
  registros: Activity[];
}

export interface Activity {
  id: string;
  paciente: Patient;
  inicio: Date;
  termino: Date;
  tipoAtividade: string;
  medicos: Doctor[];
}

export type ActivityDataPayload = ListActivitiesDto /*| ActivityDetailsDto*/;
