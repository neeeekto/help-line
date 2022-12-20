import { createApiClassBySchema } from '@help-line/modules/http';
import { UsersClientApiSchema } from './schema';

export class UsersClientApi extends createApiClassBySchema(
  UsersClientApiSchema
) {}
