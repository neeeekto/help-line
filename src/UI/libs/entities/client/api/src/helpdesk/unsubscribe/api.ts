import { createApiClassBySchema } from '@help-line/modules/http';
import { UnsubscribeClientApiSchema } from './schema';

export class UnsubscribeClientApi extends createApiClassBySchema(
  UnsubscribeClientApiSchema
) {}
