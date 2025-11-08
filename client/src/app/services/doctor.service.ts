import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Doctor, ListDoctorsDto } from '../models/doctor.models';
import { mapApiReponse } from '../utils/map-api-response';
import { ApiResponseDto } from '../models/api.models';

@Injectable({ providedIn: 'root' })
export class DoctorService {
  private readonly apiUrl: string = environment.apiUrl + '/api/medicos';
  private readonly http: HttpClient = inject(HttpClient);

  public getAll(): Observable<Doctor[]> {
    return this.http.get<ApiResponseDto>(this.apiUrl).pipe(
      map(mapApiReponse<ListDoctorsDto>),
      map((res) => res.registros),
    );
  }
}
