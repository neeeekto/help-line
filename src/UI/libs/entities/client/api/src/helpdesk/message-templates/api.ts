import { MessageTemplateClientApiSchema } from './schema';
import { createApiClassBySchema } from '@help-line/modules/http';

export class MessageTemplateClientApi extends createApiClassBySchema(
  MessageTemplateClientApiSchema
) {}
