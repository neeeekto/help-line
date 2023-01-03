import { createStubApi } from '@help-line/dev/http-stubs';
import {
  TagsClientApiSchema,
  TagDescriptionsClientApiSchema,
} from '@help-line/entities/client/api';

export const helpdeskTagsClientStubApi = createStubApi(TagsClientApiSchema);
export const helpdeskTagsDescClientStubApi = createStubApi(
  TagDescriptionsClientApiSchema
);
