import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import {
  Patient,
  PatientDto,
  IdApiResponse,
  PatientDetailsDto,
  ListPatientsDto,
} from '../models/patient.models';
import { mapApiResponse } from '../utils/map-api-response';
import { ApiResponseDto } from '../models/api.models';

@Injectable({
  providedIn: 'root', 
})
export class PatientService {
  private readonly apiUrl: string = environment.apiUrl + '/api/pacientes';
  private readonly http: HttpClient = inject(HttpClient);

  public register(registerModel: PatientDto): Observable<IdApiResponse> {
    return this.http.post<IdApiResponse>(this.apiUrl, registerModel);
  }
  public edit(id: string, editModel: PatientDto): Observable<IdApiResponse> {
    const url = `${this.apiUrl}/${id}`;

    return this.http.put<IdApiResponse>(url, editModel);
  }
  public delete(id: string): Observable<null> {
    const url = `${this.apiUrl}/${id}`;

    return this.http.delete<null>(url);
  }
  public getById(id: string): Observable<PatientDetailsDto> {
    const url = `${this.apiUrl}/${id}`;

    return this.http.get<ApiResponseDto>(url).pipe(map(mapApiResponse<PatientDetailsDto>));
  }
  public getAll(): Observable<Patient[]> {
    return this.http.get<ApiResponseDto>(this.apiUrl).pipe(
      map(mapApiResponse<ListPatientsDto>),
      map((res) => res.registros),
    );
  }
}
