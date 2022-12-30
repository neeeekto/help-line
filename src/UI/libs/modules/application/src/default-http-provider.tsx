import React, {
  PropsWithChildren,
  useEffect,
  useMemo,
  useRef,
  useState,
} from 'react';
import { useAuthUser, useLogoutByNetworkAction } from '@help-line/modules/auth';
import { HttpInterceptor } from '@help-line/modules/http';
import {
  ApiResolverInterceptor,
  AuthFacade,
  AuthInterceptor,
  HttpProvider,
} from '@help-line/modules/api';
import { IEnvironment } from './environment.type';

interface DefaultHttpProviderProps extends PropsWithChildren {
  interceptors?: HttpInterceptor[];
  config: {
    serverUrl: string;
    apiPrefix: string;
  };
}

export const DefaultHttpProvider: React.FC<DefaultHttpProviderProps> = ({
  children,
  config,
  interceptors: additionalInterceptors,
}) => {
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

  const [interceptors] = useState([
    new ApiResolverInterceptor(config.apiPrefix, config.serverUrl),
    new AuthInterceptor(authFacadeRef.current),
    ...(additionalInterceptors ?? []),
  ]);
  return (
    <HttpProvider interceptors={interceptors} serverUrl={config.serverUrl}>
      {children}
    </HttpProvider>
  );
};
