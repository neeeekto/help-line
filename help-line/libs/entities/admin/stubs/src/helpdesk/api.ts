import { createStubApi } from '@help-line/modules/http-stubs';
import { HelpdeskAdminApiSchema } from '@help-line/entities/admin/api';

export const adminHelpdeskStubApi = createStubApi(HelpdeskAdminApiSchema);
