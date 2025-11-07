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

export interface Patient {
  id: string;
  nome: string;
  cpf: string;
  email: string;
  telefone: string;
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

export interface Doctor {
  id: string;
  nome: string;
  crm: string;
}

export interface PatientApiResponseDto {
  sucesso: boolean;
  dados: PatientDataPayload;
}

export type PatientApiResponse =
  | { sucesso: true; dados: PatientDataPayload }
  | { sucesso: false; erro: string[] };

export type PatientDataPayload = ListPatientsDto | IdApiResponse | PatientDetailsDto;
