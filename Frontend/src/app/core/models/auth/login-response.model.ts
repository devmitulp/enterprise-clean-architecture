export interface LoginResponse {
  AccessToken: string;
  RefreshToken: string;
  RequiresMfa?: boolean;
  MfaToken?: string;
}
