import React, { useEffect, useMemo, useState } from "react";
import { SystemProvider } from "@core/system/components";
import {
  AuthProvider,
  HelpLineUserProfile,
  HelpLineUserProfileProjectPermissions,
} from "@core/auth";
import { QueryClientForTest } from "@test-utils/query-client-for-test";
import { AuthStoreContext } from "@core/auth/auth.context";
import { makeTestAuthStore } from "@test-utils/fakes/test-auth-store";
import { makeSystemStore } from "@core/system/system.store";
import { SystemStoreContext } from "@core/system/system.context";
import { Project } from "@entities/helpdesk/projects";
import { User as HLUser } from "@entities/user-access/users";

export const StoryBookRoot: React.FC<{
  profile?:
    | Partial<HelpLineUserProfile>
    | Partial<HelpLineUserProfileProjectPermissions>;
  me?: Partial<HLUser> | null;
  project?: Project;
}> = ({ children, profile, project, me }) => {
  const authStore = useMemo(() => makeTestAuthStore(profile, me), []);
  const systemStore = useMemo(() => {
    const store = makeSystemStore();
    if (project) {
      store.setProject(project);
    }
    return store;
  }, []);

  return (
    <AuthStoreContext.Provider value={authStore}>
      <SystemStoreContext.Provider value={systemStore}>
        {children}
      </SystemStoreContext.Provider>
    </AuthStoreContext.Provider>
  );
};
