import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Doctor, DoctorDto, ListDoctorsDto } from '../models/doctor.models';
import { mapApiReponse } from '../utils/map-api-response';
import { ApiResponseDto, IdApiResponse } from '../models/api.models';

@Injectable({ providedIn: 'root' })
export class DoctorService {
  private readonly apiUrl: string = environment.apiUrl + '/api/medicos';
  private readonly http: HttpClient = inject(HttpClient);

  public register(registerModel: DoctorDto): Observable<IdApiResponse> {
    return this.http.post<IdApiResponse>(this.apiUrl, registerModel);
  }
  public getAll(): Observable<Doctor[]> {
    return this.http.get<ApiResponseDto>(this.apiUrl).pipe(
      map(mapApiReponse<ListDoctorsDto>),
      map((res) => res.registros),
    );
  }
}
