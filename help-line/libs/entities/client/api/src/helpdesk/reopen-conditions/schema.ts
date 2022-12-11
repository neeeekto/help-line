import { makeRudSchema } from '../../api.presets';
import { ReopenCondition, ReopenConditionData } from './types';
import { makeHeaderWithProject, ProjectApiRequest } from '../../schema.share';
import { createApiAction, HttpMethod } from '@help-line/modules/http';

export const ReopenConditionsClientApiSchema = {
  ...makeRudSchema<
    ReopenCondition,
    ReopenConditionData,
    ReopenCondition['id'],
    ProjectApiRequest
  >('/v1/hd/reopen-conditions', makeHeaderWithProject),
  switch: createApiAction<
    void,
    { fromId: ReopenCondition['id']; toId: ReopenCondition['id'] }
  >({
    method: HttpMethod.POST,
    url: ({ fromId }) => `/v1/hd/reopen-conditions/${fromId}/switch`,
    data: ({ toId }) => toId,
  }),
  toggle: createApiAction<void, { reopenConditionId: string }>({
    method: HttpMethod.PATCH,
    url: ({ reopenConditionId }) =>
      `/v1/hd/reopen-conditions/${reopenConditionId}/toggle`,
  }),
};
