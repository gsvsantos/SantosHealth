import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Doctor, DoctorDto, DoctorDtoToTop10, ListDoctorsDto, Top10DoctorsDto } from '../models/doctor.models';
import { mapApiReponse } from '../utils/map-api-response';
import { ApiResponseDto, IdApiResponse } from '../models/api.models';

@Injectable({ providedIn: 'root' })
export class DoctorService {
  private readonly apiUrl: string = environment.apiUrl + '/api/medicos';
  private readonly http: HttpClient = inject(HttpClient);

  public register(registerModel: DoctorDto): Observable<IdApiResponse> {
    return this.http.post<IdApiResponse>(this.apiUrl, registerModel);
  }
  public edit(id: string, editModel: DoctorDto): Observable<IdApiResponse> {
    const url = `${this.apiUrl}/${id}`;

    return this.http.put<IdApiResponse>(url, editModel);
  }
  public delete(id: string): Observable<null> {
    const url = `${this.apiUrl}/${id}`;

    return this.http.delete<null>(url);
  }
  public getById(id: string): Observable<Doctor> {
    const url = `${this.apiUrl}/${id}`;

    return this.http.get<ApiResponseDto>(url).pipe(map(mapApiReponse<Doctor>));
  }
  public getAll(): Observable<Doctor[]> {
    return this.http.get<ApiResponseDto>(this.apiUrl).pipe(
      map(mapApiReponse<ListDoctorsDto>),
      map((res) => res.registros),
    );
  }
  public getTop10(
    startDate: string | null,
    endDate: string | null,
  ): Observable<DoctorDtoToTop10[]> {
    const url = `${this.apiUrl}/top-10`;

    let params = new HttpParams();
    if (startDate != null) {
      params = params.set('inicioPeriodo', startDate);
    }
    if (endDate != null) {
      params = params.set('inicioPeriodo', endDate);
    }

    return this.http.get<ApiResponseDto>(url, { params }).pipe(
      map(mapApiReponse<Top10DoctorsDto>),
      map((res) => res.registros),
    );
  }
}
