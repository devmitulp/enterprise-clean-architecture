import { UserContext } from '../../auth/user-context.interface';

export interface LoginResponse {
  AccessToken?: string;
  RefreshToken?: string;
  RequiresMfa?: boolean;
  IsMfaSetupRequired?: boolean;
  MfaToken?: string;
  QrCodeSvg?: string;
  Secret?: string;
  UserContext?: UserContext;
}
