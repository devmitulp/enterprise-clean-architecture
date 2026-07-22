export interface UserContext {
  Id: string | null;
  UserName: string | null;
  EmailAddress: string | null;
  FirstName: string | null;
  LastName: string | null;
  FullName: string | null;
  Roles: string[];
  Permissions?: string[];
  TenantId: string | null;
  Department: string | null;
  IsMfaEnabled?: boolean;
}
