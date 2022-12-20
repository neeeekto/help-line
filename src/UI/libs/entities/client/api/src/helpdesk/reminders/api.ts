import { createApiClassBySchema } from '@help-line/modules/http';
import { RemindersClientApiSchema } from './schema';

export class RemindersClientApi extends createApiClassBySchema(
  RemindersClientApiSchema
) {}
