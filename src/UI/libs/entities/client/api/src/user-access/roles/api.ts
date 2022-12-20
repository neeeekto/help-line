import { createApiClassBySchema } from '@help-line/modules/http';
import { RolesClientApiSchema } from './schema';

export class RolesClientApi extends createApiClassBySchema(
  RolesClientApiSchema
) {}
