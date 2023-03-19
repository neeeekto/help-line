import { observable, action, computed, values } from 'mobx';
import { computedFn } from 'mobx-utils';
import { User } from 'oidc-client';

export const makeAuthStore = () => {
  const state = observable({
    isAuth: false,
    user: null as null | User,
  });

  return {
    state,
    selectors: {},
    actions: {},
  };
};

export type AuthStore = ReturnType<typeof makeAuthStore>;
