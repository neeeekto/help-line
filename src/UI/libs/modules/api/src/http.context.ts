import { createContext } from 'react';
import { HttpClient } from '@help-line/modules/http';

export const HttpContext = createContext<HttpClient>(null!);
