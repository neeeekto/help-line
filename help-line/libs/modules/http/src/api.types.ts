import { HttpMethod, ResponseType } from './http.types';
export type ValueOrFactory<TValue, TParams> =
  | TValue
  | ((params: TParams) => TValue);

export interface IApiAction<TReq, TRes> {
  readonly method: HttpMethod;
  readonly url: ValueOrFactory<string, TReq>;
  readonly header?: ValueOrFactory<Record<string, any>, TReq>;
  readonly params?: ValueOrFactory<Record<string, any>, TReq>;
  readonly data?: (req: TReq) => any;
  readonly responseType?: ResponseType;
}
