import React, { PropsWithChildren, useMemo } from 'react';
import { AuthStoreContext, AuthEventContext } from '../context';
import { makeAuthEvents } from '../events';
import { makeAuthStore } from '../store';
import { UserManager, UserManagerSettings } from 'oidc-client';

export const AuthProvider: React.FC<
  PropsWithChildren<{ settings: UserManagerSettings }>
> = React.memo(({ children, settings }) => {
  const authEvents = useMemo(() => makeAuthEvents(), []);
  const oauthManager = useMemo(() => new UserManager(settings), [settings]);
  const authStore = useMemo(
    () => makeAuthStore(oauthManager, authEvents),
    [oauthManager, authEvents]
  );
  return (
    <AuthEventContext.Provider value={authEvents}>
      <AuthStoreContext.Provider value={authStore}>
        {children}
      </AuthStoreContext.Provider>
    </AuthEventContext.Provider>
  );
});
