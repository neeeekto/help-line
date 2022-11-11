import { MockedRequest, ResponseResolver, RestContext } from 'msw';

export const makeSuccessResponse =
  <TResult>(
    result: TResult,
    once = false,
    code = 200
  ): ResponseResolver<MockedRequest, RestContext, TResult> =>
  (req, res, ctx) => {
    const resFn = once ? res.once : res;
    return resFn(ctx.json(result), ctx.status(code));
  };

export const makeErrorResponse =
  (
    code: number,
    error?: any,
    once = false
  ): ResponseResolver<MockedRequest, RestContext> =>
  (req, res, ctx) => {
    const resFn = once ? res.once : res;
    return resFn(ctx.status(code), ctx.json(error));
  };
