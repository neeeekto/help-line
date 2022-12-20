import { createApiClassBySchema } from '@help-line/modules/http';
import { ProjectsClientApiSchema } from './schema';

export class ProjectsClientApi extends createApiClassBySchema(
  ProjectsClientApiSchema
) {}
