import { createContext } from 'react';
import { AuthStore } from './store';
import { Events } from './events';

export const AuthStoreContext = createContext<AuthStore>(null!);
export const AuthEventContext = createContext<Events>(null!);
