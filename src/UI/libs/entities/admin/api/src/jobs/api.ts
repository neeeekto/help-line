import { createApiClassBySchema } from '@help-line/modules/http';
import { JobsAdminApiSchema } from './schema';

export class JobsAdminApi extends createApiClassBySchema(JobsAdminApiSchema) {}
