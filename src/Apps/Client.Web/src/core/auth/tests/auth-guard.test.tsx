import { AuthStore, Login, makeAuthEvents, makeAuthStore } from "@core/auth";
import { AuthGuard } from "../components/auth-guard";
import { MockComponent } from "@test-utils/mock-component";
import { AuthStoreContext } from "@core/auth/auth.context";
import { mockStore } from "@test-utils/mock-store.utils";
import { InitView } from "@core/system/components";
import React from "react";
import { makeElementByTypeFilter } from "@test-utils/find.utils";
import { renderAndGet } from "@test-utils/render.utils";
import {makeTestAuthStore} from "@test-utils/fakes/test-auth-store";

describe("AuthGuard", () => {
  const renderGuard = (authStore: AuthStore) =>
    renderAndGet(
      <AuthStoreContext.Provider value={authStore}>
        <AuthGuard>
          <MockComponent />
        </AuthGuard>
      </AuthStoreContext.Provider>
    );

  it("call init method of authStore", async () => {
    const mockedStore = mockStore(makeTestAuthStore(), {
      init: (mock) => mock.mockReturnValue(new Promise(() => {})),
    });
    const renderer = renderGuard(mockedStore.store);

    expect(mockedStore.mocks.init).toBeCalled();
  });

  it("has loading screen", async () => {
    const authStore = makeTestAuthStore();
    authStore.state.loading = true;
    const renderResult = await renderGuard(authStore);

    expect(
      renderResult.root.findAll(makeElementByTypeFilter(Login)).length
    ).toBe(0);
    expect(renderResult.root.findAllByType(MockComponent).length).toBe(0);
    expect(renderResult.root.findAllByType(InitView).length).toBe(1);
  });

  it("show login view if not authorized", async () => {
    const authStore = makeTestAuthStore();
    authStore.state.loading = false;
    authStore.state.isAuth = false;
    const renderer = await renderGuard(authStore);
    expect(renderer.root.findAll(makeElementByTypeFilter(Login)).length).toBe(
      1
    );
  });

  it("show content if authorized", async () => {
    const authStore = makeTestAuthStore();
    authStore.state.loading = false;
    authStore.state.isAuth = true;
    const renderer = await renderGuard(authStore);
    expect(
      renderer.root.findAll(makeElementByTypeFilter(MockComponent)).length
    ).toBe(1);
  });
});
