/* eslint-disable @typescript-eslint/no-non-null-assertion */
import { createContext } from 'react';

// eslint-disable-next-line @typescript-eslint/ban-types,@typescript-eslint/no-explicit-any
export type ApiCache = Map<any, any>;
export const ApiContext = createContext<ApiCache>(null!);

// eslint-disable-next-line @typescript-eslint/ban-types,@typescript-eslint/no-explicit-any
export type ApiFactoryRegistry = Map<any, any>;
export const ApiFactoryContext = createContext<ApiFactoryRegistry>(null!);

// eslint-disable-next-line @typescript-eslint/ban-types,@typescript-eslint/no-explicit-any
export type ApiHttpFactoryRegistry = Map<any, any>;
export const ApiHttpFactoryContext = createContext<ApiHttpFactoryRegistry>(
  null!
);
