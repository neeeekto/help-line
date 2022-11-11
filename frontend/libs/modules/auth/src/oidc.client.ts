import { UserManager, UserManagerSettings } from 'oidc-client';

export const makeUseManager = (settings: UserManagerSettings) => {
  return new UserManager(settings);
};
