import {createContext, useContext} from "react";
import {SystemStore} from "@core/system/system.store";

export const SystemStoreContext = createContext<SystemStore>(null as any);

export const useSystemStore$ = () => useContext(SystemStoreContext);
