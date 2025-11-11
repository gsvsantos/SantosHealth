import { HttpErrorResponse } from '@angular/common/http';
import { ApiResponseDto } from '../models/api.models';
import { Observable, throwError } from 'rxjs';

export function mapApiResponse<T>(res: ApiResponseDto): T {
  if (!res.sucesso) throw new Error(res.erros.join('. '));

  return res.dados as T;
}

export function mapApiErroResponse(res: HttpErrorResponse): Observable<never> | null {
  const obj = res.error as ApiResponseDto;
  if (obj.sucesso) return null;

  return throwError(() => obj.erros?.join('. '));
}
