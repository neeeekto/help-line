import {
  createApiAction,
  HttpMethod,
  IApiAction,
} from '@help-line/modules/http';
import { CreateProjectData, Project, ProjectData } from './types';

export const ProjectAdminApiSchema = {
  get: createApiAction<Project[], void>({
    method: HttpMethod.GET,
    url: '/v1/projects/',
  }),

  create: createApiAction<void, CreateProjectData>({
    method: HttpMethod.POST,
    url: `/v1/projects/`,
    data: (req) => req,
  }),

  update: createApiAction<
    void,
    { projectId: Project['id']; data: ProjectData }
  >({
    method: HttpMethod.PATCH,
    url: ({ projectId }) => `/v1/projects/${projectId}/`,
    data: ({ data }) => data,
  }),

  deactivate: createApiAction<void, { projectId: Project['id'] }>({
    method: HttpMethod.POST,
    url: ({ projectId }) => `/v1/projects/${projectId}/deactivate/`,
  }),

  activate: createApiAction<void, { projectId: Project['id'] }>({
    method: HttpMethod.POST,
    url: ({ projectId }) => `/v1/projects/${projectId}/activate/`,
  }),
};
