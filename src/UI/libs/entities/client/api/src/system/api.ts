import { createApiClassBySchema } from '@help-line/modules/http';
import { SystemClientApiSchema } from './schema';

export class SystemClientApi extends createApiClassBySchema(
  SystemClientApiSchema
) {}
