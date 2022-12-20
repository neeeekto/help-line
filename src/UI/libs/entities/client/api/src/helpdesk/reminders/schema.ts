import { createApiAction, HttpMethod } from '@help-line/modules/http';
import { SavedReminder, SavedReminderData } from './types';
import { makeCrudSchema } from '../../api.presets';
import { makeHeaderWithProject, ProjectApiRequest } from '../../schema.share';

export const RemindersClientApiSchema = {
  ...makeCrudSchema<
    SavedReminder,
    SavedReminderData,
    SavedReminderData,
    SavedReminder['id'],
    ProjectApiRequest
  >('/v1/hd/reminders', makeHeaderWithProject),
  get: createApiAction<
    SavedReminder[],
    { enabled?: boolean } & ProjectApiRequest
  >({
    method: HttpMethod.GET,
    header: makeHeaderWithProject,
    url: `/v1/hd/reminders`,
    params: (req) => ({
      enabled: req.enabled,
    }),
  }),
};
