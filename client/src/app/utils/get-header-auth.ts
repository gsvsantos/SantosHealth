import { AccessTokenModel } from '../models/auth.models';

export function getHeaderAuthorizationOptions(accessToken?: AccessTokenModel): object {
  if (!accessToken) throw new Error('The access token was not provided');

  return {
    headers: {
      Authorization: 'Bearer ' + accessToken.key,
    },
  };
}
