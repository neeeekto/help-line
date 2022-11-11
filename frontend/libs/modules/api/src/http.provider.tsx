import React, { PropsWithChildren, useMemo } from 'react';
import { HttpClient } from '@help-line/http';
import { AxiosHttpBackend, HttpInterceptor } from '@help-line/http';
import { HttpContext } from './index';
import { ApiContext } from './api.context';
import {
  ApiFactoryContext,
  ApiHttpFactoryContext,
} from './api-factory.context';

interface HttpProviderProps extends PropsWithChildren {
  serverUrl?: string;
  interceptors?: HttpInterceptor[];
}

export const HttpProvider: React.FC<HttpProviderProps> = React.memo((props) => {
  const httpClient = useMemo(
    () =>
      new HttpClient(
        new AxiosHttpBackend(props.serverUrl || ''),
        props.interceptors || []
      ),
    [props.interceptors, props.serverUrl]
  );

  // eslint-disable-next-line react-hooks/exhaustive-deps
  const apiCache = useMemo(() => new Map(), [httpClient]);
  // eslint-disable-next-line react-hooks/exhaustive-deps
  const apiFactory = useMemo(() => new Map(), [httpClient]);
  // eslint-disable-next-line react-hooks/exhaustive-deps
  const apiHttpFactory = useMemo(() => new Map(), [httpClient]);

  return (
    <HttpContext.Provider value={httpClient}>
      <ApiHttpFactoryContext.Provider value={apiHttpFactory}>
        <ApiFactoryContext.Provider value={apiFactory}>
          <ApiContext.Provider value={apiCache}>
            {props.children}
          </ApiContext.Provider>
        </ApiFactoryContext.Provider>
      </ApiHttpFactoryContext.Provider>
    </HttpContext.Provider>
  );
});
