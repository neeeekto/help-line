type WatcherFn = (status: boolean) => void;
export const makeAuthEvents = () => {
  const watchers: WatcherFn[] = [];

  const subscribe = (fn: WatcherFn) => {
    watchers.push(fn);
  };

  const emit = (status: boolean) => {
    watchers.forEach((fn) => fn(status));
  };

  return {
    subscribe,
    emit,
  };
};
export type AuthEvents = ReturnType<typeof makeAuthEvents>;
