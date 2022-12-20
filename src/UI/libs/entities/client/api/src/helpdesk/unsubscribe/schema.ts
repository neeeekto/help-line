import { makeRudSchema } from '../../api.presets';
import {
  makeHeaderWithProject,
  ProjectApiRequest,
  Tag,
  Unsubscribe,
} from '@help-line/entities/client/api';
import { createApiAction, HttpMethod } from '@help-line/modules/http';

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
