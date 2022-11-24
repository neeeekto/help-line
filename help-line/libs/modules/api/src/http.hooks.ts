import { useContext } from 'react';
import { HttpContext } from './http.context';

export const useHttpClient = () => useContext(HttpContext);
