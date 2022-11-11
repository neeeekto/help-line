import { ApiFactory } from '@help-line/http';
import {
  DefaultBodyType,
  MockedRequest,
  ResponseResolver,
  rest,
  RestContext,
  RestHandler,
} from 'msw';

type FunctionType<TRes = any> = (...args: any[]) => Promise<TRes>;
type StubFactory<TRes = any> = (
  handler: ResponseResolver<MockedRequest, RestContext, TRes>
) => RestHandler<MockedRequest<DefaultBodyType>>;

type Setups<T extends Record<string, FunctionType>> = {
  [method in keyof T]: StubFactory<Awaited<ReturnType<T[method]>>>;
};

const createFactory = <TResult>(url: string, method: keyof typeof rest) => {
  function factory(
    handler: ResponseResolver<MockedRequest, RestContext, TResult>
  ) {
    return rest[method](url, handler);
  }
  factory.url = url;

  return factory;
};

export const createGetStubFactory = <TResult>(url: string) =>
  createFactory<TResult>(url, 'get');

export const createPostStubFactory = <TResult>(url: string) =>
  createFactory<TResult>(url, 'post');

export const createPutStubFactory = <TResult>(url: string) =>
  createFactory<TResult>(url, 'put');

export const createPathStubFactory = <TResult>(url: string) =>
  createFactory<TResult>(url, 'patch');

export const createDeleteStubFactory = <TResult>(url: string) =>
  createFactory<TResult>(url, 'delete');

export const createFakeApi = <T extends Record<string, FunctionType>>(
  apiFatory: ApiFactory<T>,
  setups: Setups<T>
) => {
  return setups;
};

export const extractUrlFromStubFactory = (fn: any) => {
  return (fn as any)?.url;
};
