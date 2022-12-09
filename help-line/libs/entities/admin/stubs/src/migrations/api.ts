import { createStubApi } from '@help-line/modules/http-stubs';
import { MigrationsAdminApiSchema } from '@help-line/entities/admin/api';

export const adminMigrationsStubApi = createStubApi(MigrationsAdminApiSchema);
