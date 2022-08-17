import { createContext } from "react";

export const ApiRegistryContext = createContext<Map<Function, any>>(
  new Map<Function, any>()
);
