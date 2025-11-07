import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { PatientApiResponse, Patient, PatientDto, ListPatientsDto, IdApiResponse } from '../models/patient.models';

@Injectable({
  providedIn: 'root',
})
export class PatientService {
  private readonly apiUrl: string = environment.apiUrl + '/api/pacientes';
  private readonly http: HttpClient = inject(HttpClient);

  public register(registerModel: PatientDto): Observable<IdApiResponse> {
    return this.http.post<IdApiResponse>(this.apiUrl, registerModel);
  }
  public edit(): void {}
  public delete(): void {}
  public getById(id: string): void {}
  public getAll(): Observable<Patient[]> {
    return this.http.get<PatientApiResponse>(this.apiUrl).pipe(
      map((res) => {
        const lista = res.dados as ListPatientsDto;
        return lista.registros;
      }),
    );
  }
}
