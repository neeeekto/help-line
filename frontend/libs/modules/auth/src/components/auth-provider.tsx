import React, { PropsWithChildren, useMemo } from 'react';
import { AuthStoreContext, AuthEventContext } from '../auth.context';
import { makeUseManager } from '../oidc.client';
import { makeAuthEvents } from '../auth.events';
import { makeAuthStore } from '../auth.store';
import { UserManagerSettings } from 'oidc-client';

export const AuthProvider: React.FC<
  PropsWithChildren<{ settings: UserManagerSettings }>
> = React.memo(({ children, settings }) => {
  const authEvents = useMemo(() => makeAuthEvents(), []);
  const oauthManager = useMemo(() => makeUseManager(settings), [settings]);
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
