import { IApiAction } from '@help-line/modules/http';
import {
  DefaultBodyType,
  MockedRequest,
  ResponseResolver,
  rest,
  RestContext,
  RestHandler,
} from 'msw';
import { callOrGetValue } from '@help-line/modules/common';
import { DeepPartial } from 'ts-essentials';

type StubApiBySchema<TSchema extends Record<string, IApiAction<any, any>>> = {
  [K in keyof TSchema]: TSchema[K] extends IApiAction<infer TReq, infer TRes>
    ? (
        handler: ResponseResolver<
          MockedRequest,
          RestContext,
          TRes & DefaultBodyType
        >,
        req: DeepPartial<TReq>
      ) => RestHandler<MockedRequest<TRes & DefaultBodyType>>
    : unknown;
};

export const createStubApiSetuper = <
  TSchema extends Record<string, IApiAction<any, any>>
>(
  schema: TSchema
) => {
  const result: any = {};

  Object.entries(schema).forEach(([key, action]) => {
    result[key] = (
      handler: ResponseResolver<MockedRequest, RestContext, DefaultBodyType>,
      req: any
    ) => {
      const url = callOrGetValue(action.url, req);
      const params = callOrGetValue(action.params, req);
      const mockUrl = [url, new URLSearchParams(params).toString()].join('?');
      return rest[action.method.toLowerCase() as any as keyof typeof rest](
        mockUrl,
        handler
      );
    };
  });
  return result as StubApiBySchema<TSchema>;
};

export class ApiStubBuilder<
  TSchema extends Record<string, IApiAction<any, any>>
> {
  private readonly schema: TSchema;

  constructor(schema: TSchema) {
    this.schema = schema;
  }

  build() {}
}
