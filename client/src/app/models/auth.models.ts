export interface RegisterModel {
  userName: string;
  email: string;
  password: string;
}

export interface LoginModel {
  email: string;
  senha: string;
}

export interface AuthApiResponse {
  sucesso: boolean;
  dados: AccessTokenDto;
}

export interface AccessTokenDto {
  chave: string;
  dataExpiracao: string;
  usuario: {
    id: string;
    userName: string;
    email: string;
  };
}

export interface AccessTokenModel {
  key: string;
  expiration: Date;
  authenticatedUser: {
    id: string;
    userName: string;
    email: string;
  };
}

export interface AuthenticatedUserModel {
  id: string;
  userName: string;
  email: string;
}
