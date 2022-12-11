import { createApiClassBySchema } from '@help-line/modules/http';
import { RemindersClientApiSchema } from '../reminders';
import { ReopenConditionsClientApiSchema } from './schema';

export class ReopenConditionsClientApi extends createApiClassBySchema(
  ReopenConditionsClientApiSchema
) {}
