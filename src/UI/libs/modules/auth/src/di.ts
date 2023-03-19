import { UserManager, UserManagerSettings } from 'oidc-client';
import { DiRegistratorFn } from '@help-line/modules/di';
import { AuthStore } from './store';

export const registryAuthInDI =
  (settings: UserManagerSettings): DiRegistratorFn =>
  (container) => {
    container.bind(UserManager).toConstantValue(new UserManager(settings));
    container
      .bind(AuthStore)
      .toConstantValue(new AuthStore(container.resolve(UserManager)));
  };
