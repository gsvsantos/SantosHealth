import { ActivityDto, EditActivityDto, ListActivitiesDto } from './../models/activity.models';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Activity } from '../models/activity.models';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { ApiResponseDto, IdApiResponse } from '../models/api.models';
import { mapApiReponse } from '../utils/map-api-response';

@Injectable({
  providedIn: 'root',
})
export class ActivityService {
  private readonly apiUrl: string = environment.apiUrl + '/api/atividades-medicas';
  private readonly http: HttpClient = inject(HttpClient);

  public register(registerModel: ActivityDto): Observable<IdApiResponse> {
    return this.http.post<IdApiResponse>(this.apiUrl, registerModel);
  }
  public edit(id: string, editModel: EditActivityDto): Observable<IdApiResponse> {
    const url = `${this.apiUrl}/${id}`;

    return this.http.put<IdApiResponse>(url, editModel);
  }
  public getById(id: string): Observable<Activity> {
    const url = `${this.apiUrl}/${id}`;

    return this.http.get<ApiResponseDto>(url).pipe(map(mapApiReponse<Activity>));
  }
  public getAll(activityType: string | null): Observable<Activity[]> {
    let params = new HttpParams();
    if (activityType != null) {
      params = params.set('tipoAtividade', activityType);
    }
    console.log(activityType);
    return this.http.get<ApiResponseDto>(this.apiUrl, { params }).pipe(
      map(mapApiReponse<ListActivitiesDto>),
      map((res) => res.registros),
    );
  }
}
