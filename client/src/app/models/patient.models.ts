export interface PatientDto {
  nome: string;
  cpf: string;
  email: string;
  telefone: string;
}

export interface IdApiResponse {
  id: string;
}

export interface ListPatientsApiResponse {
  sucesso: boolean;
  dados: ListPatientsDto;
}

export interface ListPatientsDto {
  quantidadeRegistros: number;
  registros: Patient[];
}

export interface PatientDetailsApiResponse extends Patient {
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
