import { AuthStoreContext } from "../auth.context";
import { Login, makeAuthEvents, makeAuthStore } from "@core/auth";
import { mockStore } from "@test-utils/mock-store.utils";
import { cleanup, render, fireEvent, screen } from "@testing-library/react";
import { makeTestAuthStore } from "@test-utils/fakes/test-auth-store";

describe("Login", () => {
  it("mock", () => {
    expect(true).toBeTruthy();
  });
});
