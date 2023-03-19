import { createContext } from 'react';
import { Events } from './events';
import { UserManager } from 'oidc-client';

export const AuthUserManagerContext = createContext<UserManager>(null!);
export const AuthEventContext = createContext<Events>(null!);
