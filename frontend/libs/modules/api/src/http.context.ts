import { createContext } from 'react';
import { HttpClient } from '@help-line/http';

export const HttpContext = createContext<HttpClient>(null!);
