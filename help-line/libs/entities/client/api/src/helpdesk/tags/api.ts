import { createApiClassBySchema } from '@help-line/modules/http';
import { TagDescriptionsClientApiSchema, TagsClientApiSchema } from './schema';

export class TagsClientApi extends createApiClassBySchema(
  TagsClientApiSchema
) {}
export class TagDescriptionsClientApi extends createApiClassBySchema(
  TagDescriptionsClientApiSchema
) {}
