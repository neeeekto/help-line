import { useAuthUser, useLogoutByNetworkAction } from '@help-line/modules/auth';
import { useEffect, useRef } from 'react';
import { AuthFacade } from '../interceptors';

export const useAuthHttpAccessorRef = () => {
  const user = useAuthUser();
  const logoutByNetworkAction = useLogoutByNetworkAction();
  const authFacadeRef = useRef<AuthFacade>({
    getToken: () => null,
    logout: () => Promise.resolve(),
  });
  useEffect(() => {
    authFacadeRef.current.getToken = () => {
      return user
        ? {
            type: user?.token_type,
            value: user?.access_token,
          }
        : null;
    };
    authFacadeRef.current.logout = () => logoutByNetworkAction.mutateAsync();
  }, [user, logoutByNetworkAction]);

  return authFacadeRef.current;
};
