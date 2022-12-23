import { createApiClassBySchema } from '@help-line/modules/http';
import { HelpdeskAdminApiSchema } from './schema';

export class HelpdeskAdminApi extends createApiClassBySchema(
  HelpdeskAdminApiSchema
) {}
