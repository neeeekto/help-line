import { createApiClassBySchema } from '@help-line/modules/http';
import { MigrationsAdminApiSchema } from './schema';

export class MigrationAdminApi extends createApiClassBySchema(
  MigrationsAdminApiSchema
) {}
