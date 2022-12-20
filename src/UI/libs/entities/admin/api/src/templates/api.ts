import { createApiClassBySchema } from '@help-line/modules/http';
import {
  ComponentAdminApiSchema,
  ContextAdminApiSchema,
  TemplateAdminApiSchema,
} from './schema';

export class TemplateAdminApi extends createApiClassBySchema(
  TemplateAdminApiSchema
) {}

export class ContextAdminApi extends createApiClassBySchema(
  ContextAdminApiSchema
) {}

export class ComponentAdminApi extends createApiClassBySchema(
  ComponentAdminApiSchema
) {}
