import { useContext } from 'react';
import { AuthUserManagerContext } from './context';
import { HelpLineUserProfile } from './types';
import { useAuthProfile } from './store';

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

export const useCheckPermission = (project: string) => {
  const profile = useAuthProfile();
  return (
    permissionsOrPermission: string | string[],
    params?: PermissionCheckerParams
  ) => checkerPermission(permissionsOrPermission, project, profile, params);
};

export const useHasPermission = (
  project: string,
  permissionsOrPermission: string | string[],
  params?: PermissionCheckerParams
) => {
  const checker = useCheckPermission(project);
  return checker(permissionsOrPermission, params);
};
