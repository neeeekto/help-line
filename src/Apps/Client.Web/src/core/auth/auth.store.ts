import { observable, action, computed } from "mobx";

import { UserManager, User } from "oidc-client";
import { makeAuthEvents } from "./auth.events";
import { User as HLUser } from "@entities/user-access/users";
import { HelpLineUserProfile } from "@core/auth/auth.types";
import { authApi } from "@core/auth/auth.api";
import { AxiosError } from "axios";

export const makeAuthStore = (
  userManager: UserManager,
  authEvents: ReturnType<typeof makeAuthEvents>
) => {
  const state = observable({
    loading: false,
    isAuth: true,
    user: null as User | null,
    me: null as HLUser | null,
  });

  const profile = computed(
    () => state.user?.profile as HelpLineUserProfile | null
  );

  const login = action("login", async () => {
    state.loading = true;
    await userManager.signinPopup();
  });

  const logoutLocal = action("logoutLocal", async () => {
    await userManager.clearStaleState();
    await userManager.removeUser();
  });

  const logoutGlobal = action("logoutGlobal", async () => {
    await userManager.signoutPopup();
  });

  const init = action("init", async () => {
    state.loading = true;
    const user = await userManager.getUser();
    if (user) {
      userManager.events.load(user);
    } else {
      state.loading = false;
    }
  });

  userManager.events.addUserLoaded(async (user) => {
    state.user = user;
    try {
      state.me = await authApi.me();
      state.isAuth = true;
      authEvents.set(true);
    } catch (e) {
      state.isAuth = false;
      authEvents.set(false);
    } finally {
      state.loading = false;
    }
  });

  userManager.events.addUserUnloaded(async () => {
    state.isAuth = false;
    authEvents.set(false);
  });

  return {
    state,
    profile,
    login,
    init,
    logoutLocal,
    logoutGlobal,
  };
};

export type AuthStore = ReturnType<typeof makeAuthStore>;
