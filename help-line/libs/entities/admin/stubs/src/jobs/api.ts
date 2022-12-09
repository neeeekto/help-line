import { createStubApi } from '@help-line/modules/http-stubs';
import {
  JobsAdminApiSchema,
  ProjectAdminApiSchema,
} from '@help-line/entities/admin/api';

export const adminJobsStubApi = createStubApi(JobsAdminApiSchema);
