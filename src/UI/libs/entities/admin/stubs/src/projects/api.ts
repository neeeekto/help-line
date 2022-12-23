import { createStubApi } from '@help-line/dev/http-stubs';
import { ProjectAdminApiSchema } from '@help-line/entities/admin/api';

export const adminProjectsStubApi = createStubApi(ProjectAdminApiSchema);
