import { ApiBase } from '@help-line/http';
import { Job, JobData, JobTriggerState } from './types';
import { Description } from '@help-line/api/share';

export class JobsAdminApi extends ApiBase {
  public async get() {
    return this.http.get<Job[]>('/api/v1/jobs/').then((x) => x.data);
  }
  public async getById(jobId: Job['id']) {
    return this.http.get<Job>(`/api/v1/jobs/${jobId}/`).then((x) => x.data);
  }
  public async create(task: string, data: JobData) {
    return this.http
      .post<Job['id']>(`/api/v1/jobs/${task}/`, data)
      .then((x) => x.data);
  }
  public async update(jobId: Job['id'], data: JobData) {
    return this.http.patch(`/api/v1/jobs/${jobId}/`, data).then((x) => x.data);
  }
  public async delete(jobId: Job['id']) {
    return this.http.delete(`/api/v1/jobs/${jobId}/`).then((x) => x.data);
  }
  public async toggle(jobId: Job['id'], enable: boolean) {
    return this.http
      .post(`/api/v1/jobs/toggle/${jobId}/?enable=${enable}`)
      .then((x) => x.data);
  }
  public async getTasks() {
    return this.http
      .get<Record<string, Description>>(`/api/v1/jobs/tasks/`)
      .then((x) => x.data);
  }
  public async fire(jobId: Job['id']) {
    return this.http.post(`/api/v1/jobs/fire/${jobId}/`).then((x) => x.data);
  }
  public async getState(jobIds: Array<Job['id']>) {
    return this.http
      .post<Record<Job['id'], JobTriggerState>>(`/api/v1/jobs/state/`, jobIds)
      .then((x) => x.data);
  }
}
