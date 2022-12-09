import { IApiAction } from './api.types';

export const createApiAction = <TRes, TReq = void>(
  config: IApiAction<TReq, TRes>
) => config;
