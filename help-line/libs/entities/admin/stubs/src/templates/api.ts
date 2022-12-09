import { createStubApi } from '@help-line/modules/http-stubs';
import {
  ComponentAdminApiSchema,
  ContextAdminApiSchema,
  TemplateAdminApiSchema,
} from '@help-line/entities/admin/api';

export const adminTemplatesStubApi = createStubApi(TemplateAdminApiSchema);
export const adminContextsStubApi = createStubApi(ContextAdminApiSchema);
export const adminComponentsStubApi = createStubApi(ComponentAdminApiSchema);
