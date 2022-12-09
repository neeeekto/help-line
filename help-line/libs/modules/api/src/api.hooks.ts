import { HttpClient } from '@help-line/modules/http';
import { useContext, useEffect } from 'react';
import {
  ApiContext,
  ApiFactoryContext,
  ApiHttpFactoryContext,
} from './api.context';
import { useHttpClient } from './http.hooks';

type ApiCctor<TApi> = new (http: HttpClient) => TApi;

export const useApi = <TApi>(
  apiCctor: ApiCctor<TApi>,
  factory?: (http: HttpClient) => TApi
): TApi => {
  const defaultHttp = useHttpClient();
  const customHttp = useContext(ApiHttpFactoryContext);
  const apiCache = useContext(ApiContext);
  const apiFactory = useContext(ApiFactoryContext);

  if (!defaultHttp) {
    throw new Error('http context not found');
  }

  if (!apiCache) {
    throw new Error('apiCache context not found');
  }

  let api = apiCache.get(apiCctor);
  const http = customHttp.get(apiCctor) || defaultHttp;
  if (!api) {
    factory = factory || apiFactory.get(apiCctor);
    api = factory ? factory(http) : new apiCctor(http);
    apiCache.set(apiCctor, api);
  }
  return api;
};

export const useCustomHttpClientForApi = <TApi>(
  apiCctor: ApiCctor<TApi>,
  factory: (http: HttpClient) => HttpClient
) => {
  const http = useHttpClient();
  const registry = useContext(ApiHttpFactoryContext);
  if (!http) {
    throw new Error('http context not found');
  }

  if (!registry) {
    throw new Error('ApiHttpFactoryContext not found');
  }

  useEffect(() => {
    registry.set(apiCctor, factory(http));
  }, [http, registry]);
};

export const useCustomApiFactory = <TApi>(
  apiCctor: ApiCctor<TApi>,
  factory: (http: HttpClient) => TApi
) => {
  const registry = useContext(ApiFactoryContext);

  if (!registry) {
    throw new Error('ApiFactoryContext not found');
  }

  useEffect(() => {
    registry.set(apiCctor, factory);
  }, [apiCctor, registry]);
};
