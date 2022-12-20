import { makeAuthEvents } from "../auth.events";

describe("auth.events", () => {
  it("set_publishStatue_success", () => {
    const authEvents = makeAuthEvents();
    const fn = jest.fn();
    authEvents.on(fn);
    authEvents.set(false);

    expect(fn).toHaveBeenCalledWith(false);
  });

  it("on_allSubscribesWillCalls_success", () => {
    const authEvents = makeAuthEvents();
    const fn1 = jest.fn();
    const fn2 = jest.fn();
    authEvents.on(fn1);
    authEvents.on(fn2);
    authEvents.set(false);

    expect(fn1).toHaveBeenCalledWith(false);
    expect(fn2).toHaveBeenCalledWith(false);
  });

  it("on_subscribesWillCallsEveryTimes_success", () => {
    const authEvents = makeAuthEvents();
    const fn = jest.fn();
    authEvents.on(fn);
    authEvents.set(false);

    expect(fn).toHaveBeenCalledWith(false);

    authEvents.set(true);
    expect(fn).toHaveBeenCalledWith(true);
  });
});
