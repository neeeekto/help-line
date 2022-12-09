import { createApiAction, HttpMethod } from '@help-line/modules/http';
import { Migration } from './types';
import { Description, WithType } from '@help-line/entities/share';

export const MigrationsAdminApiSchema = {
  get: createApiAction<Migration[], void>({
    method: HttpMethod.GET,
    url: '/v1/migrations/',
  }),
  run: createApiAction<
    Migration[],
    { migration: Migration['name']; params?: WithType<string> }
  >({
    method: HttpMethod.POST,
    url: '/v1/migrations/',
    header: {
      Accept: 'application/json',
      'Content-Type': 'application/json',
    },
  }),

  getParamsDescriptions: createApiAction<Record<string, Description>, void>({
    method: HttpMethod.GET,
    url: '/v1/migrations/',
  }),
};
