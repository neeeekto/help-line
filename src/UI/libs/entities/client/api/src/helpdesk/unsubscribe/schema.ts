import { Unsubscribe } from './types';
import { createApiAction, HttpMethod } from '@help-line/modules/http';
import { makeHeaderWithProject, ProjectApiRequest } from '../../schema.share';

export const UnsubscribeClientApiSchema = {
  get: createApiAction<Unsubscribe[], ProjectApiRequest>({
    method: HttpMethod.GET,
    header: makeHeaderWithProject,
    url: `/v1/hd/unsubscribe`,
  }),
  delete: createApiAction<void, { unsubscribeId: Unsubscribe['id'] }>({
    method: HttpMethod.DELETE,
    header: makeHeaderWithProject,
    url: ({ unsubscribeId }) => `/api/v1/hd/unsubscribe/${unsubscribeId}`,
  }),
};
