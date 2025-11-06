import { Injectable } from '@angular/core';
import { AccessTokenModel } from '../models/auth.models';

@Injectable({ providedIn: 'root' })
export class LocalStorageService {
  private readonly accessTokenKey: string = 'santoshealth:access-token';

  public saveAccessToken(token: AccessTokenModel): void {
    const jsonString = JSON.stringify(token);

    localStorage.setItem(this.accessTokenKey, jsonString);
  }

  public clearAccessToken(): void {
    localStorage.removeItem(this.accessTokenKey);
  }

  public getAccessToken(): AccessTokenModel | undefined {
    const jsonString = localStorage.getItem(this.accessTokenKey);

    if (!jsonString) return undefined;

    return JSON.parse(jsonString) as AccessTokenModel;
  }
}
