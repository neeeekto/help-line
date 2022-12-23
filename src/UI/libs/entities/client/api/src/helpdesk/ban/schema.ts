import { createApiAction, HttpMethod } from '@help-line/modules/http';
import { Ban, CreateBanData } from './types';
import { makeHeaderWithProject, ProjectApiRequest } from '../../schema.share';

export const BanClientApiSchema = {
  get: createApiAction<Ban[], ProjectApiRequest>({
    method: HttpMethod.GET,
    url: '/v1/hd/ban/',
    header: makeHeaderWithProject,
  }),

  add: createApiAction<Ban['id'], { data: CreateBanData } & ProjectApiRequest>({
    method: HttpMethod.POST,
    url: '/v1/hd/ban/',
    data: ({ data }) => data,
    header: makeHeaderWithProject,
  }),

  delete: createApiAction<void, { banId: Ban['id'] } & ProjectApiRequest>({
    method: HttpMethod.DELETE,
    url: ({ banId }) => `/v1/hd/ban/${banId}`,
    header: makeHeaderWithProject,
  }),
};
