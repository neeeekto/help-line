import { httpClient, HttpClient } from "@core/http";
import { Job, JobData, JobTriggerState } from "./types";
import { Description } from "@entities/meta.types";

// eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types
export const makeJobsApi = (http: HttpClient) => ({
  get: () => http.get<Job[]>("/api/v1/jobs").then((x) => x.data),
  getById: (jobId: Job["id"]) =>
    http.get<Job>(`/api/v1/jobs/${jobId}`).then((x) => x.data),
  create: (task: string, data: JobData) =>
    http.post<Job["id"]>(`/api/v1/jobs/${task}`, data).then((x) => x.data),
  update: (jobId: Job["id"], data: JobData) =>
    http.patch(`/api/v1/jobs/${jobId}`, data).then((x) => x.data),
  delete: (jobId: Job["id"]) =>
    http.delete(`/api/v1/jobs/${jobId}`).then((x) => x.data),
  toggle: (jobId: Job["id"], enable: boolean) =>
    http
      .post(`/api/v1/jobs/toggle/${jobId}?enable=${enable}`)
      .then((x) => x.data),
  getTasks: () =>
    http
      .get<Record<string, Description>>(`/api/v1/jobs/tasks`)
      .then((x) => x.data),
  fire: (jobId: Job["id"]) =>
    http.post(`/api/v1/jobs/fire/${jobId}`).then((x) => x.data),
  getState: (jobIds: Array<Job["id"]>) =>
    http
      .post<Record<Job["id"], JobTriggerState>>(`/api/v1/jobs/state`, jobIds)
      .then((x) => x.data),
});

export const jobsApi = makeJobsApi(httpClient);
