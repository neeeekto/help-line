import { useContext } from "react";
import { AuthStoreContext } from "./auth.context";
import { useSystemStore$ } from "@core/system";
import { HelpLineUserProfile } from "@core/auth/auth.types";

export const useAuthStore$ = () => useContext(AuthStoreContext);

export type PermissionCheckerParams = Partial<{
  all: boolean;
  ignoreProject: boolean;
}>;

export const permissionChecker = (
  permissionsOrPermission: string | string[],
  currentProject: string,
  profile?: HelpLineUserProfile | null,
  params?: PermissionCheckerParams
) => {
  if (!profile) {
    return false;
  }
  if (profile.isAdmin) {
    return true;
  }

  const permissions = Array.isArray(permissionsOrPermission)
    ? permissionsOrPermission
    : [permissionsOrPermission];

  const projectPermissions = Object.keys(profile)
    .filter((key) => key.includes(`${currentProject}.permission`))
    .map((x) => profile[x])
    .flat(1);

  const commonPermissions = profile.permission || [];
  const permissionsForChecking = commonPermissions
    ? [...commonPermissions]
    : [];
  if (!params?.ignoreProject) {
    permissionsForChecking.push(...projectPermissions);
  }
  const checkFn = (permission: string) =>
    permissionsForChecking.includes(permission);
  return params?.all ? permissions.every(checkFn) : permissions.some(checkFn);
};

export const usePermissionChecker$ = () => {
  const authStore = useAuthStore$();
  const systemStore = useSystemStore$();
  const profile = authStore.profile.get();
  return (
    permissionsOrPermission: string | string[],
    params?: PermissionCheckerParams
  ) =>
    permissionChecker(
      permissionsOrPermission,
      systemStore.state.currentProject!,
      profile,
      params
    );
};

export const useHasPermission$ = (
  permissionsOrPermission: string | string[],
  params?: PermissionCheckerParams
) => {
  const checker = usePermissionChecker$();
  return checker(permissionsOrPermission, params);
};
