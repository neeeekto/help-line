import { createContext, useContext } from "react";
import { AuthStore } from "./auth.store";
import { AuthEvents } from "./auth.events";

export const AuthStoreContext = createContext<AuthStore>(null!);
export const AuthEventContext = createContext<AuthEvents>(null!);

export const useAuthStore$ = () => useContext(AuthStoreContext);
