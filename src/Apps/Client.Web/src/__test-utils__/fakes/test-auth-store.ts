import { action, computed, observable } from "mobx";
import { User } from "oidc-client";
import { User as HLUser } from "@entities/user-access/users";
import {
  AuthStore,
  HelpLineUserProfile,
  HelpLineUserProfileProjectPermissions,
} from "@core/auth";
import { EN_LANGUAGE } from "../data/project.test-data";

export const makeTestAuthStore = (
  profileData:
    | Partial<HelpLineUserProfile>
    | Partial<HelpLineUserProfileProjectPermissions> = {},
  me?: Partial<HLUser> | null
): AuthStore => {
  const state = observable({
    loading: true,
    isAuth: false,
    user: null as User | null,
    me: (me || null) as HLUser | null,
  });

  const profile = computed(
    () =>
      ({
        firstName: "test",
        lastName: "test",
        language: EN_LANGUAGE,
        photo: "",
        permission: [],
        userId: "",
        isAdmin: false,
        ...profileData,
      } as HelpLineUserProfile)
  );

  const login = action("login", async () => {});

  const logout = action("logout", async () => {});

  const init = action("init", async () => {});

  return {
    state,
    profile,
    login,
    init,
    logoutLocal: logout,
    logoutGlobal: logout,
  };
};
