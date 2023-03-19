import { createApiClassBySchema } from '@help-line/modules/http';
import { ReopenConditionsClientApiSchema } from './schema';

export class ReopenConditionsClientApi extends createApiClassBySchema(
  ReopenConditionsClientApiSchema
) {}
