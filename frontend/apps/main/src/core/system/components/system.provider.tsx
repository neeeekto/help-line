import React, { PropsWithChildren, useMemo } from 'react';
import { SystemStoreContext } from '../system.context';
import { makeSystemStore } from '../system.store';

export const SystemProvider: React.FC<PropsWithChildren> = ({ children }) => {
  const store = useMemo(() => makeSystemStore(), []);
  return (
    <SystemStoreContext.Provider value={store}>
      {children}
    </SystemStoreContext.Provider>
  );
};
