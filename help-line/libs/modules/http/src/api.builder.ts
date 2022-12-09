import { callOrGetValue } from '@help-line/modules/common';
import { IApiAction } from './api.types';
import { HttpClient } from './http.client';

export type ApiByScheme<TSchema extends Record<string, IApiAction<any, any>>> =
  {
    [K in keyof TSchema]: TSchema[K] extends IApiAction<infer TReq, infer TRes>
      ? (req: TReq) => Promise<TRes>
      : unknown;
  };

export interface ApiBySchemaConstructor<
  TSchema extends Record<string, IApiAction<any, any>>
> {
  new (http: HttpClient): ApiByScheme<TSchema>;
}

export function createApiClassBySchema<
  TSchema extends Record<string, IApiAction<any, any>>
>(schema: TSchema) {
  const ApiBySchema = class {
    constructor(private readonly http: HttpClient) {}
  };

  Object.entries(schema).forEach(([key, action]) => {
    // @ts-ignore
    ApiBySchema.prototype[key] = function (req: any) {
      // @ts-ignore
      this.http
        .fetch({
          url: callOrGetValue(action.url, req),
          params: callOrGetValue(action.params, req),
          data: callOrGetValue(action.data, req),
          headers: callOrGetValue(action.header, req),
          method: action.method,
          responseType: action.responseType,
        })
        .then((x) => x.data);
    };
  });

  return ApiBySchema as any as ApiBySchemaConstructor<TSchema>;
}
