import { ApiResponseDto } from "../models/api.models";

export function mapApiReponse<T>(res: ApiResponseDto): T {
  if (!res.sucesso) throw new Error(res.erro.join('. '));

  return res.dados as T;
}
