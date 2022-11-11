import { observable, action } from 'mobx';

import { UserManager, User } from 'oidc-client';
import { makeAuthEvents } from './auth.events';

export const makeAuthStore = (
  userManager: UserManager,
  authEvents: ReturnType<typeof makeAuthEvents>
) => {
  const state = observable({
    isAuth: false,
    user: null as User | null,
  });

  const login = action('login', async () => {
    await userManager.signinPopup();
  });

  const logoutLocal = action('logoutLocal', async () => {
    await userManager.clearStaleState();
    await userManager.removeUser();
  });

  const logoutGlobal = action('logoutGlobal', async () => {
    await userManager.signoutPopup();
  });

  const init = action('init', async () => {
    const user = await userManager.getUser();
    if (user) {
      userManager.events.load(user);
    }
  });

  userManager.events.addUserLoaded((user) => {
    state.isAuth = true;
    state.user = user;
    console.log(user);
    authEvents.emit(true);
  });

  userManager.events.addUserUnloaded(() => {
    state.isAuth = false;
    authEvents.emit(false);
  });

  return {
    state,
    login,
    init,
    logoutLocal,
    logoutGlobal,
  };
};

export type AuthStore = ReturnType<typeof makeAuthStore>;
