import React, { useMemo, useRef } from "react";
import { ApiRegistryContext } from "@core/http/api.context";

export const ApiProvider: React.FC = React.memo(({ children }) => {
  const registry = useMemo(() => new Map<Function, any>(), []);
  return (
    <ApiRegistryContext.Provider value={registry}>
      {children}
    </ApiRegistryContext.Provider>
  );
});
