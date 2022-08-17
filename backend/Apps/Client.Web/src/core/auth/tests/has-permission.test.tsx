import { AuthStore, Login, makeAuthEvents, makeAuthStore } from "@core/auth";
import { MockComponent } from "@test-utils/mock-component";
import { AuthStoreContext } from "@core/auth/auth.context";
import { mockStore } from "@test-utils/mock-store.utils";
import { renderAndGet } from "@test-utils/render.utils";
import React from "react";
import { HasPermission } from "@core/auth/components";
import { ArgumentTypes } from "@core/types";
import { SystemStoreContext } from "@core/system/system.context";
import { makeSystemStore, SystemStore } from "@core/system/system.store";
import {makeTestAuthStore} from "@test-utils/fakes/test-auth-store";

const DeniedView: React.FC = () => <div>DeniedView</div>;
const TEST_KEY = "test";
describe("HasPermission", () => {
  const renderComponent = (
    authStore: AuthStore,
    systemStore: SystemStore,
    params: ArgumentTypes<typeof HasPermission>[0]
  ) =>
    renderAndGet(
      <AuthStoreContext.Provider value={authStore}>
        <SystemStoreContext.Provider value={systemStore}>
          <HasPermission {...params}>
            <MockComponent />
          </HasPermission>
        </SystemStoreContext.Provider>
      </AuthStoreContext.Provider>
    );

  it("show content if user has access", async () => {
    const systemStore = makeSystemStore();
    const mockedStore = makeTestAuthStore({permission: [TEST_KEY]})
    mockedStore.state.isAuth = true;
    const rendered = await renderComponent(mockedStore, systemStore, {
      permissions: TEST_KEY,
      deniedView: <DeniedView />,
    });

    expect(rendered.root.findAllByType(MockComponent).length).toBe(1);
    expect(rendered.root.findAllByType(DeniedView).length).toBe(0);
  });

  it("show deniedView if user has no access", async () => {
    const systemStore = makeSystemStore();
    const mockedStore = makeTestAuthStore({permission: []})
    mockedStore.state.isAuth = true;

    const rendered = await renderComponent(mockedStore, systemStore, {
      permissions: TEST_KEY,
      deniedView: <DeniedView />,
    });

    expect(rendered.root.findAllByType(MockComponent).length).toBe(0);
    expect(rendered.root.findAllByType(DeniedView).length).toBe(1);
  });
});
