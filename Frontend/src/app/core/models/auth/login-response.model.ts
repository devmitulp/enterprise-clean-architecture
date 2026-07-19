import { UserContext } from '../../auth/user-context.interface';

export interface LoginResponse {
  AccessToken?: string;
  RefreshToken?: string;
  RequiresMfa?: boolean;
  MfaToken?: string;
  UserContext?: UserContext;
}
