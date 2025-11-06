import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import {
  BehaviorSubject,
  defer,
  distinctUntilChanged,
  merge,
  Observable,
  of,
  shareReplay,
  skip,
  tap,
} from 'rxjs';
import { AccessTokenModel, LoginModel, RegisterModel } from '../models/auth.models';
import { LocalStorageService } from './local-storage.service';
import { getHeaderAuthorizationOptions } from '../utils/get-header-auth';

@Injectable()
export class AuthService {
  private readonly localStorage = inject(LocalStorageService);
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl + '/auth';

  public readonly accessTokenSubject$ = new BehaviorSubject<AccessTokenModel | undefined>(
    undefined,
  );

  public readonly storedAccessToken$ = defer(() => {
    const accessToken = this.localStorage.getAccessToken();

    if (!accessToken) return of(undefined);

    const isValid = new Date(accessToken.expiration) > new Date();

    if (!isValid) return of(undefined);

    return of(accessToken);
  });

  public readonly accessToken$: Observable<AccessTokenModel | undefined> = merge(
    this.storedAccessToken$,
    this.accessTokenSubject$.pipe(skip(1)),
  ).pipe(
    distinctUntilChanged((first, second) => first === second),
    tap((accessToken) => {
      if (accessToken) this.localStorage.saveAccessToken(accessToken);
      else this.localStorage.clearAccessToken();

      this.accessTokenSubject$.next(accessToken);
    }),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  public register(model: RegisterModel): Observable<AccessTokenModel> {
    const url = `${this.apiUrl}/registro`;

    return this.http
      .post<AccessTokenModel>(url, model)
      .pipe(tap((token) => this.accessTokenSubject$.next(token)));
  }

  public login(loginModel: LoginModel): Observable<AccessTokenModel> {
    const url = `${this.apiUrl}/login`;

    return this.http
      .post<AccessTokenModel>(url, loginModel)
      .pipe(tap((token) => this.accessTokenSubject$.next(token)));
  }

  public logout(): Observable<null> {
    const urlCompleto = `${this.apiUrl}/sair`;

    return this.http
      .post<null>(
        urlCompleto,
        {},
        getHeaderAuthorizationOptions(this.accessTokenSubject$.getValue()),
      )
      .pipe(tap(() => this.accessTokenSubject$.next(undefined)));
  }
}
