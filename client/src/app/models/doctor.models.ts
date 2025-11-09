export interface DoctorDto {
  nome: string;
  crm: string;
}

export interface ListDoctorsDto {
  quantidadeRegistros: number;
  registros: Doctor[];
}

export interface Doctor extends DoctorDto {
  id: string;
}

export interface Top10DoctorsDto {
  quantidadeRegistros: number;
  registros: DoctorDtoToTop10[];
}

export interface DoctorDtoToTop10 {
  medicoId: string;
  medico: string;
  totalDeHorasTrabalhadas: number;
}

export type DoctorDataPayload = ListDoctorsDto | Doctor;
