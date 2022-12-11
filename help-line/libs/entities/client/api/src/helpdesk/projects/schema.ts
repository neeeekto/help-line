import { createApiAction, HttpMethod } from '@help-line/modules/http';
import { Project } from './types';

export const ProjectsClientApiSchema = {
  get: createApiAction<Project[]>({
    method: HttpMethod.GET,
    url: '/v1/hd/projects/',
  }),
};
