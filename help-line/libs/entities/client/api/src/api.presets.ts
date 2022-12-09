import {
  createApiAction,
  HttpMethod,
  ValueOrFactory,
} from '@help-line/modules/http';
import { callOrGetValue } from '@help-line/modules/common';

const preparePrefix = (apiPrefix: string) => {
  if (apiPrefix.endsWith('/')) {
    apiPrefix = apiPrefix.slice(0, -1);
  }
  return apiPrefix;
};

export const makeRudSchema = <TEntity, TSaveData, TId, TShareArgs>(
  apiPrefix: string,
  header?: ValueOrFactory<Record<string, any>, TShareArgs>
) => {
  apiPrefix = preparePrefix(apiPrefix);

  return {
    get: createApiAction<TEntity[], TShareArgs>({
      method: HttpMethod.GET,
      url: `${apiPrefix}/`,
      header,
    }),

    delete: createApiAction<void, { id: TId } & TShareArgs>({
      method: HttpMethod.DELETE,
      url: ({ id }) => `${apiPrefix}/${id}/`,
    }),
    save: createApiAction<void, { id: TId; data: TSaveData } & TShareArgs>({
      method: HttpMethod.PUT,
      url: ({ id }) => `${apiPrefix}/${id}/`,
      data: ({ data }) => data,
      header: (req) => ({
        ...callOrGetValue(header, req),
        Accept: 'application/json',
        'Content-Type': 'application/json',
      }),
    }),
  };
};

export const makeCrudSchema = <
  TEntity,
  TCreateData,
  TUpdateData,
  TId,
  TShareArgs
>(
  apiPrefix: string,
  header?: ValueOrFactory<Record<string, any>, TShareArgs>
) => {
  apiPrefix = preparePrefix(apiPrefix);

  return {
    get: createApiAction<TEntity[], TShareArgs>({
      method: HttpMethod.GET,
      url: `${apiPrefix}/`,
      header,
    }),

    delete: createApiAction<void, { id: TId } & TShareArgs>({
      method: HttpMethod.DELETE,
      url: ({ id }) => `${apiPrefix}/${id}/`,
    }),
    create: createApiAction<void, { data: TCreateData } & TShareArgs>({
      method: HttpMethod.POST,
      url: `${apiPrefix}/`,
      data: ({ data }) => data,
      header: (req) => ({
        ...callOrGetValue(header, req),
        Accept: 'application/json',
        'Content-Type': 'application/json',
      }),
    }),
    update: createApiAction<void, { id: TId; data: TUpdateData } & TShareArgs>({
      method: HttpMethod.PATCH,
      url: ({ id }) => `${apiPrefix}/${id}/`,
      data: ({ data }) => data,
      header: (req) => ({
        ...callOrGetValue(header, req),
        Accept: 'application/json',
        'Content-Type': 'application/json',
      }),
    }),
  };
};
