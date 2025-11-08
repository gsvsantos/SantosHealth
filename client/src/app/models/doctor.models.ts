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

export type DoctorDataPayload = ListDoctorsDto | Doctor;
