import { CreateProjectData, Project, ProjectData } from './types';
import { ApiBase } from '@help-line/http';

export class ProjectAdminApi extends ApiBase {
  public async get() {
    return this.http.get<Project[]>(`/v1/projects/`).then((x) => x.data);
  }
  public async create(data: CreateProjectData) {
    return this.http
      .post<Project['id']>(`/v1/projects/`, data)
      .then((x) => x.data);
  }
  public async update(projectId: Project['id'], data: ProjectData) {
    return this.http
      .patch<void>(`/v1/projects/${projectId}/`, data)
      .then((x) => x.data);
  }
  public async deactivate(projectId: Project['id']) {
    return this.http
      .post<void>(`/v1/projects/${projectId}/deactivate/`)
      .then((x) => x.data);
  }
  public async activate(projectId: Project['id']) {
    return this.http
      .post<void>(`/v1/projects/${projectId}/activate/`)
      .then((x) => x.data);
  }
}
