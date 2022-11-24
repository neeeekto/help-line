import { useContext } from 'react';
import { AuthStoreContext } from './context';

export const useAuthStore$ = () => useContext(AuthStoreContext);
