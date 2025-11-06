export interface RegisterModel {
  fullName: string;
  email: string;
  password: string;
}

export interface LoginModel {
  email: string;
  senha: string;
}

export interface AccessTokenModel {
  key: string;
  expiration: Date;
  authenticatedUser: AuthenticatedUserModel;
}

export interface AuthenticatedUserModel {
  id: string;
  fullName: string;
  email: string;
}
