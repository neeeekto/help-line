import React, { PropsWithChildren, useMemo, useState } from 'react';
import { useAuthStore$ } from '@help-line/modules/auth';
import {
  AuthFacade,
  AuthInterceptor,
  HttpInterceptor,
  HttpProvider,
  PrefixInterceptor,
} from '@help-line/modules/http';
import { IEnvironment } from './environment.type';

interface DefaultHttpProviderProps extends PropsWithChildren {
  env: IEnvironment;
  interceptors?: HttpInterceptor[];
}

export const DefaultHttpProvider: React.FC<DefaultHttpProviderProps> = ({
  children,
  env,
  interceptors: additionalInterceptors,
}) => {
  const authStore$ = useAuthStore$();
  const authFacade = useMemo(() => {
    return {
      getToken: () => {
        const user = authStore$.state?.user;
        return user
          ? {
              type: user.token_type,
              value: user.access_token,
            }
          : null;
      },
      logout: () => {
        return authStore$.logoutLocal();
      },
    } as AuthFacade;
  }, [authStore$]);

  const [interceptors] = useState([
    new PrefixInterceptor(env.apiPrefix, env.serverUrl),
    new AuthInterceptor(authFacade),
    ...(additionalInterceptors ?? []),
  ]);
  return <HttpProvider interceptors={interceptors}>{children}</HttpProvider>;
};
