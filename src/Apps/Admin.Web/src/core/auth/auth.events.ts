type WatcherFn = (status: boolean) => void;
export const makeAuthEvents = () => {
  const watchers: WatcherFn[] = [];

  const on = (fn: WatcherFn) => {
    watchers.push(fn);
  };

  const set = (status: boolean) => {
    watchers.forEach((fn) => fn(status));
  };

  return {
    on,
    set,
  };
};
export type AuthEvents = ReturnType<typeof makeAuthEvents>;
