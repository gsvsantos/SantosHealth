import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { ListPatientsApiResponse, Patient } from '../models/patient.models';

@Injectable({
  providedIn: 'root',
})
export class PatientService {
  private readonly apiUrl: string = environment.apiUrl + '/api/pacientes';
  private readonly http: HttpClient = inject(HttpClient);

  public register(): void {}
  public edit(): void {}
  public delete(): void {}
  public getById(id: string): void {}
  public getAll(): Observable<Patient[]> {
    return this.http
      .get<ListPatientsApiResponse>(this.apiUrl)
      .pipe(map((res) => res.dados.registros));
  }
}
