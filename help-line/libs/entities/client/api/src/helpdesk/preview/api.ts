import { createApiClassBySchema } from '@help-line/modules/http';
import { PreviewClientApiSchema } from './schema';

export class PreviewClientApi extends createApiClassBySchema(
  PreviewClientApiSchema
) {}
