import { httpClient, HttpClient } from "@core/http";
import { CreateProjectData, Project, ProjectData } from "./types";

// eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types
export const makeProjectApi = (http: HttpClient) => ({
  get: () => http.get<Project[]>(`/api/v1/projects`).then((x) => x.data),
  create: (data: CreateProjectData) =>
    http.post<Project["id"]>(`/api/v1/projects`, data).then((x) => x.data),
  update: (projectId: Project["id"], data: ProjectData) =>
    http.patch<void>(`/api/v1/projects/${projectId}`, data).then((x) => x.data),
  deactivate: (projectId: Project["id"]) =>
    http
      .post<void>(`/api/v1/projects/${projectId}/deactivate`)
      .then((x) => x.data),
  activate: (projectId: Project["id"]) =>
    http
      .post<void>(`/api/v1/projects/${projectId}/activate`)
      .then((x) => x.data),
});

export const projectApi = makeProjectApi(httpClient);
