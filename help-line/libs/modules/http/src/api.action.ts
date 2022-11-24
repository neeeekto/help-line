import { IApiAction } from './api.types';

export const createApiAction = <TRes, TReq>(config: IApiAction<TReq, TRes>) =>
  config;
