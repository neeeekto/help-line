import { ApiBuilder } from '@help-line/modules/http';
import { ProjectAdminApiSchema } from './schema';

export const ProjectAdminApi = new ApiBuilder(ProjectAdminApiSchema);
