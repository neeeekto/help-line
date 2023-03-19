import { useCallback } from 'react';
import { AuthStore } from './store';
import { useInjection } from 'inversify-react';
import { checkerPermission, PermissionCheckerParams } from './checker';

export const useAuthStore$ = () => {
  return useInjection(AuthStore);
};

export const useCheckPermission = (project: string) => {
  const store$ = useAuthStore$();

  return useCallback(
    (
      permissionsOrPermission: string | string[],
      params?: PermissionCheckerParams
    ) =>
      checkerPermission(
        permissionsOrPermission,
        project,
        store$.profile,
        params
      ),
    [store$, project]
  );
};

export const useHasPermission = (
  project: string,
  permissionsOrPermission: string | string[],
  params?: PermissionCheckerParams
) => {
  const checker = useCheckPermission(project);
  return checker(permissionsOrPermission, params);
};
