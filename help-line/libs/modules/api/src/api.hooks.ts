import {
  ApiBuilder,
  ApiByScheme,
  HttpClient,
  IApiAction,
} from '@help-line/modules/http';
import { useContext, useEffect } from 'react';
import {
  ApiContext,
  ApiFactoryContext,
  ApiHttpFactoryContext,
} from './api.context';
import { useHttpClient } from './http.hooks';

type ApiType<TFactory> = TFactory extends new (http: HttpClient) => infer TApi
  ? TApi
  : TFactory extends ApiBuilder<infer TSchema>
  ? ApiByScheme<TSchema>
  : unknown;

type ApiBuilderOrCctor<TApi> = new (http: HttpClient) => TApi | ApiBuilder<any>;

export const useApi = <TApi, TFactory = ApiBuilderOrCctor<TApi>>(
  apiCctorOrBuilder: TFactory,
  factory?: (http: HttpClient) => ApiType<TFactory>
): ApiType<TFactory> => {
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

  let api = apiCache.get(apiCctorOrBuilder);
  const http = customHttp.get(apiCctorOrBuilder) || defaultHttp;
  if (!api) {
    factory = factory || apiFactory.get(apiCctorOrBuilder);
    api = factory
      ? factory(http)
      : typeof apiCctorOrBuilder === 'function'
      ? new (apiCctorOrBuilder as any)(http)
      : (apiCctorOrBuilder as ApiBuilder<any>).build(http);
    apiCache.set(apiCctorOrBuilder, api);
  }
  return api;
};

export const useCustomHttpClientForApi = <
  TApi,
  TFactory = ApiBuilderOrCctor<TApi>
>(
  apiCctorOrBuilder: TFactory,
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
    registry.set(apiCctorOrBuilder, factory(http));
  }, [http, registry]);
};

export const useCustomApiFactory = <TApi, TFactory = ApiBuilderOrCctor<TApi>>(
  apiCctorOrBuilder: TFactory,
  factory: (http: HttpClient) => TApi
) => {
  const registry = useContext(ApiFactoryContext);

  if (!registry) {
    throw new Error('ApiFactoryContext not found');
  }

  useEffect(() => {
    registry.set(apiCctorOrBuilder, factory);
  }, [apiCctorOrBuilder, registry]);
};
