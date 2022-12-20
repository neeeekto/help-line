import { Profile } from 'oidc-client';

export class LoginData {
  username = '';
  password = '';
}

export type HelpLineUserProfileProjectPermissions = { [key: string]: string[] };

export interface HelpLineUserProfile
  extends Profile,
    HelpLineUserProfileProjectPermissions {
  firstName: string;
  lastName: string;
  language: string;
  photo: string;
  permission: string[];
  userId: string;
  isAdmin?: boolean;
}
