import { HttpClient } from '@help-line/http';
import { useContext } from 'react';
import {
  ApiContext,
  ApiFactoryContext,
  ApiHttpFactoryContext,
} from './api.context';
import { useHttpClient } from './http.hooks';

export const useApiClient = <T>(
  apiCctor: new (http: HttpClient) => T,
  factory?: (http: HttpClient) => T
): T => {
  const http = useHttpClient();
  const apiCache = useContext(ApiContext);
  const apiFactory = useContext(ApiFactoryContext);
  if (typeof apiCctor !== 'function') {
    throw new Error('apiFactory must be function');
  }
  if (!http) {
    throw new Error('http context not found');
  }

  if (!apiCache) {
    throw new Error('apiCache context not found');
  }

  let api = apiCache.get(apiCctor);
  if (!api) {
    factory = factory || apiFactory.get(apiCctor);
    api = factory ? factory(http) : new apiCctor(http);
    apiCache.set(apiCctor, api);
  }
  return api;
};

export const useCustomHttpClientForApi = <T>(
  apiCctor: new (http: HttpClient) => T,
  factory: (http: HttpClient) => HttpClient
) => {
  const http = useHttpClient();
  const registry = useContext(ApiHttpFactoryContext);
  if (typeof apiCctor !== 'function') {
    throw new Error('apiFactory must be function');
  }
  if (!http) {
    throw new Error('http context not found');
  }

  if (!registry) {
    throw new Error('ApiHttpFactoryContext not found');
  }

  registry.set(apiCctor, factory(http));
};

export const useCustomApiFactory = <T>(
  apiCctor: new (http: HttpClient) => T,
  factory: (http: HttpClient) => T
) => {
  const registry = useContext(ApiFactoryContext);
  if (typeof apiCctor !== 'function') {
    throw new Error('apiFactory must be function');
  }

  if (!registry) {
    throw new Error('ApiHttpFactoryContext not found');
  }

  registry.set(apiCctor, factory);
};
