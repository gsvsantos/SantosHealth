import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { map, Observable, tap } from 'rxjs';
import {
  PatientApiResponse,
  Patient,
  PatientDto,
  IdApiResponse,
  PatientDetailsDto,
  ListPatientsDto,
} from '../models/patient.models';
import { mapApiReponse } from '../utils/map-api-response';

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
  public getById(id: string): Observable<PatientDetailsDto> {
    const url = `${this.apiUrl}/${id}`;

    return this.http.get<PatientApiResponse>(url).pipe(map(mapApiReponse<PatientDetailsDto>));
  }
  public getAll(): Observable<Patient[]> {
    return this.http.get<PatientApiResponse>(this.apiUrl).pipe(
      map(mapApiReponse<ListPatientsDto>),
      map((res) => res.registros),
    );
  }
}
