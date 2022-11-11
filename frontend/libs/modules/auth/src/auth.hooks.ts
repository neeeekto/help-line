import { useContext } from 'react';
import { AuthStoreContext } from './auth.context';

export const useAuthStore$ = () => useContext(AuthStoreContext);
