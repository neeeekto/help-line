import { createContext } from 'react';
import { AuthStore } from './auth.store';
import { AuthEvents } from './auth.events';

export const AuthStoreContext = createContext<AuthStore>(null!);
export const AuthEventContext = createContext<AuthEvents>(null!);
