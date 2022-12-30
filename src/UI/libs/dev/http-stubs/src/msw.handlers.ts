import {
  DefaultBodyType,
  DelayMode,
  MockedRequest,
  ResponseResolver,
  rest,
  RestContext,
} from 'msw';
import { HttpStubRequest } from './create-stub-api';

export namespace MswHandlers {
  export const makeHandler =
    (handler: ResponseResolver<MockedRequest, RestContext, DefaultBodyType>) =>
    (req: HttpStubRequest) => {
      const mockUrl = [
        req.url,
        new URLSearchParams(req.params).toString(),
      ].join('?');
      return rest[req.method!.toLowerCase() as any as keyof typeof rest](
        mockUrl,
        handler
      );
    };

  export const success = (
    result: any,
    params?: { once?: false; code?: 200; delay?: DelayMode | number }
  ) =>
    makeHandler((req, res, ctx) => {
      const resFn = params?.once ? res.once : res;
      const handlers = [ctx.json(result), ctx.status(params?.code || 200)];
      if (params?.delay) {
        handlers.push(ctx.delay(params.delay));
      }
      return resFn(...handlers);
    });

  export const error = (code: number, error?: any, once = false) =>
    makeHandler((req, res, ctx) => {
      const resFn = once ? res.once : res;
      return resFn(ctx.status(code), ctx.json(error));
    });

  export const delay = (durationOrMode: DelayMode | number, once = false) =>
    makeHandler((req, res, ctx) => {
      const resFn = once ? res.once : res;
      return resFn(ctx.delay(durationOrMode), ctx.status(200));
    });
}
