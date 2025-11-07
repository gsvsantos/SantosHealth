import { PatientApiResponse } from '../models/patient.models';

export function mapApiReponse<T>(res: PatientApiResponse): T {
  if (!res.sucesso) throw new Error(res.erro.join('. '));

  return res.dados as T;
}
