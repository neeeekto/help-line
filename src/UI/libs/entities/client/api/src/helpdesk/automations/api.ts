import { AutoRepliesClientApiSchema } from './schema';
import { createApiClassBySchema } from '@help-line/modules/http';

export class AutoRepliesClientApi extends createApiClassBySchema(
  AutoRepliesClientApiSchema
) {}
