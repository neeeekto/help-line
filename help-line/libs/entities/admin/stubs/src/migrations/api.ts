import { createStubApi } from '@help-line/dev/http-stubs';
import { MigrationsAdminApiSchema } from '@help-line/entities/admin/api';

export const adminMigrationsStubApi = createStubApi(MigrationsAdminApiSchema);
