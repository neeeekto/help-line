import { callOrGetValue } from '@help-line/modules/common';
import { IApiAction } from './api.types';
import { HttpClient } from './http.client';

export type ApiByScheme<TSchema extends Record<string, IApiAction<any, any>>> =
  {
    [K in keyof TSchema]: TSchema[K] extends IApiAction<infer TReq, infer TRes>
      ? (req: TReq) => Promise<TRes>
      : unknown;
  };

export class ApiBuilder<TSchema extends Record<string, IApiAction<any, any>>> {
  private readonly schema: TSchema;

  constructor(schema: TSchema) {
    this.schema = schema;
  }

  public build(http: HttpClient) {
    const result: any = {};
    Object.entries(this.schema).forEach(([key, action]) => {
      result[key] = (req: any) =>
        http
          .fetch({
            url: callOrGetValue(action.url, req),
            params: callOrGetValue(action.params, req),
            data: callOrGetValue(action.data, req),
            headers: callOrGetValue(action.header, req),
            method: action.method,
            responseType: action.responseType,
          })
          .then((x) => x.data);
    });
    return result as ApiByScheme<TSchema>;
  }
}
