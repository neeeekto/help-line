import { createContext, useContext } from 'react';
import { SystemStore } from './system.store';

export const SystemStoreContext = createContext<SystemStore>(null as any);

export const useSystemStore$ = () => useContext(SystemStoreContext);
export const useCurrentProjectId$ = () => {
  const systemStore = useSystemStore$();
  return systemStore?.state.currentProject;
};
