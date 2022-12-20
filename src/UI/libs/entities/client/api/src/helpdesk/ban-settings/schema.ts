import { createApiAction, HttpMethod } from '@help-line/modules/http';
import { BanSettings } from './types';
import { makeHeaderWithProject, ProjectApiRequest } from '../../schema.share';

export const BanSettingsClientApiSchema = {
  get: createApiAction<BanSettings, ProjectApiRequest>({
    method: HttpMethod.GET,
    url: '/v1/hd/ban/settings',
    header: makeHeaderWithProject,
  }),

  set: createApiAction<void, { data: BanSettings } & ProjectApiRequest>({
    method: HttpMethod.POST,
    url: '/v1/hd/ban/settings',
    data: ({ data }) => data,
    header: makeHeaderWithProject,
  }),
};
