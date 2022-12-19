import { HttpRequest, IApiAction } from '@help-line/modules/http';
import { callOrGetValue } from '@help-line/modules/common';
import { DeepPartial, OptionalKeys, RequiredKeys } from 'ts-essentials';

export interface HttpStubRequest
  extends Readonly<
    Omit<
      HttpRequest,
      | 'withCredentials'
      | 'onDownloadProgress'
      | 'onUploadProgress'
      | 'xsrfCookieName'
      | 'xsrfHeaderName'
      | 'responseType'
    >
  > {}

export interface IStub<TReq, TRes> {
  handle<T>(handler: (req: HttpStubRequest) => T): T;
}

type StubApiBySchema<TSchema extends Record<string, IApiAction<any, any>>> = {
  [K in keyof TSchema]: TSchema[K] extends IApiAction<infer TReq, infer TRes>
    ? (req: DeepPartial<TReq>) => IStub<TReq, TRes>
    : unknown;
};

export const createStubApi = <
  TSchema extends Record<string, IApiAction<any, any>>
>(
  schema: TSchema
) => {
  const result: any = {};

  Object.entries(schema).forEach(([key, action]) => {
    result[key] = (req: any) => {
      const stubReq: HttpStubRequest = {
        method: action.method,
        data: callOrGetValue(action.data, req),
        url: callOrGetValue(action.url, req),
        params: callOrGetValue(action.params, req),
        headers: callOrGetValue(action.header, req),
      };
      return {
        handle: <T>(handler: (req: HttpStubRequest) => T) => {
          return handler(stubReq);
        },
      };
    };
  });
  return result as StubApiBySchema<TSchema>;
};
