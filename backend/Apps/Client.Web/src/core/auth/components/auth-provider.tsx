import React, { useMemo } from "react";
import { AuthStoreContext, AuthEventContext } from "../auth.context";
import { makeUseManager } from "../oidc.client";
import { makeAuthEvents } from "../auth.events";
import { makeAuthStore } from "../auth.store";

export const AuthProvider: React.FC = React.memo(({ children }) => {
  const authEvents = useMemo(() => makeAuthEvents(), []);
  const authStore = useMemo(
    () => makeAuthStore(makeUseManager(), authEvents),
    [authEvents]
  );
  return (
    <AuthEventContext.Provider value={authEvents}>
      <AuthStoreContext.Provider value={authStore}>
        {children}
      </AuthStoreContext.Provider>
    </AuthEventContext.Provider>
  );
});
