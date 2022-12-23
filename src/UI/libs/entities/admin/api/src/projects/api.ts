import { createApiClassBySchema } from '@help-line/modules/http';
import { ProjectAdminApiSchema } from './schema';

export class ProjectAdminApi extends createApiClassBySchema(
  ProjectAdminApiSchema
) {}
