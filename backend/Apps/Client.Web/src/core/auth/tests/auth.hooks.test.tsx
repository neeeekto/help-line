import { AuthStore, useHasPermission$ } from "@core/auth";
import { AuthStoreContext } from "@core/auth/auth.context";
import React from "react";
import { makeTestAuthStore } from "@test-utils/fakes/test-auth-store";
import { SystemStoreContext } from "@core/system/system.context";
import { makeSystemStore } from "@core/system/system.store";
import { renderHook } from "@testing-library/react-hooks";

const TEST_KEY = "test";
const TEST_KEY2 = "test2";
describe("useHasPermission", () => {
  const systemStore = makeSystemStore();
  systemStore.state.currentProject = TEST_KEY;

  const wrapper: React.FC<{ authStore: AuthStore }> = ({
    authStore,
    children,
  }) => (
    <AuthStoreContext.Provider value={authStore}>
      <SystemStoreContext.Provider value={systemStore}>
        {children}
      </SystemStoreContext.Provider>
    </AuthStoreContext.Provider>
  );

  it("return false if user isn't auth", () => {
    const authStore = makeTestAuthStore();
    authStore.state.isAuth = false;
    const { result } = renderHook(() => useHasPermission$(TEST_KEY), {
      wrapper,
      initialProps: { authStore },
    });

    expect(result.current).toBe(false);
  });

  it("return true if user is admin", () => {
    const authStore = makeTestAuthStore({ isAdmin: true });
    authStore.state.isAuth = true;
    const { result } = renderHook(() => useHasPermission$(TEST_KEY), {
      wrapper,
      initialProps: { authStore },
    });

    expect(result.current).toBe(true);
  });

  it("return true if user has global permission", () => {
    const authStore = makeTestAuthStore({ permission: [TEST_KEY] });
    authStore.state.isAuth = true;
    const { result } = renderHook(() => useHasPermission$(TEST_KEY), {
      wrapper,
      initialProps: { authStore },
    });

    expect(result.current).toBe(true);
  });

  it("return true if user has project permission", () => {
    const authStore = makeTestAuthStore({
      permission: [],
      [`${TEST_KEY}.permission`]: [TEST_KEY],
    });
    authStore.state.isAuth = true;

    const { result } = renderHook(() => useHasPermission$(TEST_KEY), {
      wrapper,
      initialProps: { authStore },
    });

    expect(result.current).toBe(true);
  });

  it("return false if user has only project permission and we ignore project permissions", () => {
    const authStore = makeTestAuthStore({
      permission: [],
      [`${TEST_KEY}.permission`]: [TEST_KEY],
    });
    authStore.state.isAuth = true;

    const { result } = renderHook(
      () => useHasPermission$(TEST_KEY, { ignoreProject: true }),
      {
        wrapper,
        initialProps: { authStore },
      }
    );

    expect(result.current).toBe(false);
  });

  it("return false if user has no all permissions", () => {
    const authStore = makeTestAuthStore({
      permission: [TEST_KEY],
    });
    authStore.state.isAuth = true;
    const { result } = renderHook(
      () => useHasPermission$([TEST_KEY, TEST_KEY2], { all: true }),
      {
        wrapper,
        initialProps: { authStore },
      }
    );

    expect(result.current).toBe(false);
  });

  it("return true if user has all permissions", () => {
    const authStore = makeTestAuthStore({
      permission: [TEST_KEY, TEST_KEY2],
    });
    authStore.state.isAuth = true;
    const { result } = renderHook(
      () => useHasPermission$([TEST_KEY, TEST_KEY2], { all: true }),
      {
        wrapper,
        initialProps: { authStore },
      }
    );

    expect(result.current).toBe(true);
  });

  it("return true if user has all permissions, 1 in project, 1 in global", () => {
    const authStore = makeTestAuthStore({
      permission: [TEST_KEY],
      [`${TEST_KEY}.permission`]: [TEST_KEY2],
    });
    authStore.state.isAuth = true;
    const { result } = renderHook(
      () => useHasPermission$([TEST_KEY, TEST_KEY2], { all: true }),
      {
        wrapper,
        initialProps: { authStore },
      }
    );

    expect(result.current).toBe(true);
  });
});
