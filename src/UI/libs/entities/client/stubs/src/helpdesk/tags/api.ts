import { createStubApi } from '@help-line/dev/http-stubs';
import { ProjectsClientApiSchema } from '@help-line/entities/client/api';

export const helpdeskProjectClientStubApi = createStubApi(
  ProjectsClientApiSchema
);
