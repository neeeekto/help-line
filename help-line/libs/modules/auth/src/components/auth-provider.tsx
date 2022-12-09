import React, { PropsWithChildren, useMemo } from 'react';
import { AuthUserManagerContext, AuthEventContext } from '../context';
import { makeAuthEvents } from '../events';
import { UserManager, UserManagerSettings } from 'oidc-client';

export const AuthProvider: React.FC<
  PropsWithChildren<{ settings: UserManagerSettings }>
> = React.memo(({ children, settings }) => {
  const authEvents = useMemo(() => makeAuthEvents(), []);
  const oauthManager = useMemo(() => new UserManager(settings), [settings]);
  return (
    <AuthEventContext.Provider value={authEvents}>
      <AuthUserManagerContext.Provider value={oauthManager}>
        {children}
      </AuthUserManagerContext.Provider>
    </AuthEventContext.Provider>
  );
});
