import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import {
  BehaviorSubject,
  defer,
  distinctUntilChanged,
  map,
  merge,
  Observable,
  of,
  shareReplay,
  skip,
  tap,
} from 'rxjs';
import {
  AccessTokenModel,
  AuthApiResponse,
  LoginModel,
  RegisterModel,
} from '../models/auth.models';
import { LocalStorageService } from './local-storage.service';

@Injectable()
export class AuthService {
  private readonly localStorage = inject(LocalStorageService);
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl + '/api/auth';

  public readonly accessTokenSubject$ = new BehaviorSubject<AuthApiResponse | undefined>(undefined);

  public readonly accessTokenFromApi$: Observable<AccessTokenModel | undefined> =
    this.accessTokenSubject$.pipe(
      skip(1),
      map((apiResponse) => (apiResponse ? this.mapAccessToken(apiResponse) : undefined)),
    );

  public readonly storedAccessToken$: Observable<AccessTokenModel | undefined> = defer(() => {
    const accessToken = this.localStorage.getAccessToken();

    if (!accessToken) return of(undefined);

    const isValid = new Date(accessToken.expiration) > new Date();

    if (!isValid) return of(undefined);

    return of(accessToken);
  });

  public readonly accessToken$: Observable<AccessTokenModel | undefined> = merge(
    this.storedAccessToken$,
    this.accessTokenFromApi$,
  ).pipe(
    distinctUntilChanged((prev, curr) => prev === curr),
    tap((accessToken) => {
      if (accessToken) this.localStorage.saveAccessToken(accessToken);
      else this.localStorage.clearAccessToken();
    }),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  public register(model: RegisterModel): Observable<AuthApiResponse> {
    const url = `${this.apiUrl}/registrar`;

    return this.http
      .post<AuthApiResponse>(url, model)
      .pipe(tap((token) => this.accessTokenSubject$.next(token)));
  }

  public login(loginModel: LoginModel): Observable<AuthApiResponse> {
    const url = `${this.apiUrl}/autenticar`;

    return this.http
      .post<AuthApiResponse>(url, loginModel)
      .pipe(tap((token) => this.accessTokenSubject$.next(token)));
  }

  public logout(): Observable<null> {
    const urlCompleto = `${this.apiUrl}/sair`;

    return this.http
      .post<null>(urlCompleto, {})
      .pipe(tap(() => this.accessTokenSubject$.next(undefined)));
  }

  private mapAccessToken(dto: AuthApiResponse): AccessTokenModel {
    if (!dto?.sucesso || !dto?.dados) {
      throw new Error('Resposta da API inválida');
    }

    const { chave, dataExpiracao, usuario } = dto.dados;

    return {
      key: chave,
      expiration: new Date(dataExpiracao),
      authenticatedUser: {
        id: usuario.id,
        userName: usuario.userName,
        email: usuario.email,
      },
    };
  }
}
