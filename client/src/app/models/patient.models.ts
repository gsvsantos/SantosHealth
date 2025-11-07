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

export interface PatientDetailsDto extends Patient {
  atividades: Activity[];
}

export interface Patient {
  id: string;
  nome: string;
  cpf: string;
  email: string;
  telefone: string;
}

export interface Activity {
  id: string;
  inicio: Date;
  termino: Date;
  tipoAtividade: string;
  medicos: Doctor[];
}

export interface Doctor {
  id: string;
  nome: string;
  crm: string;
}

export interface PatientApiResponse {
  sucesso: boolean;
  dados: PatientDataPayload;
}

export type PatientDataPayload = ListPatientsDto | IdApiResponse | PatientDetailsDto;
