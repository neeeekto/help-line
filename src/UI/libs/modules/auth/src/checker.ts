import { HelpLineUserProfile } from './types';

export type PermissionCheckerParams = Partial<{
  all: boolean;
  ignoreProject: boolean;
}>;

export const checkerPermission = (
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
