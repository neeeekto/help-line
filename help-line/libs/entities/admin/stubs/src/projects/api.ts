import { createStubApiSetuper } from '@help-line/modules/http-stubs';
import { ProjectAdminApiSchema } from '@help-line/entities/admin/api';

export const adminProjectsStubApi = createStubApiSetuper(ProjectAdminApiSchema);
