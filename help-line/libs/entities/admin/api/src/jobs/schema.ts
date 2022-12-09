import { createApiAction, HttpMethod } from '@help-line/modules/http';
import { Description, WithType } from '@help-line/entities/share';

import { Job, JobData, JobTriggerState } from './types';

export const JobsAdminApiSchema = {
  get: createApiAction<Job[], void>({
    method: HttpMethod.GET,
    url: '/v1/jobs/',
  }),

  getById: createApiAction<Job, { jobId: Job['id'] }>({
    method: HttpMethod.GET,
    url: ({ jobId }) => `/v1/jobs/${jobId}/`,
  }),

  create: createApiAction<Job['id'], { task: string; data: JobData }>({
    method: HttpMethod.POST,
    url: ({ task }) => `/v1/jobs/${task}/`,
    data: ({ data }) => data,
  }),

  update: createApiAction<void, { jobId: Job['id']; data: JobData }>({
    method: HttpMethod.PATCH,
    url: ({ jobId }) => `/v1/jobs/${jobId}/`,
    data: ({ data }) => data,
  }),

  delete: createApiAction<void, { jobId: Job['id'] }>({
    method: HttpMethod.DELETE,
    url: ({ jobId }) => `/v1/jobs/${jobId}/`,
  }),
  toggle: createApiAction<void, { jobId: Job['id']; enable: boolean }>({
    method: HttpMethod.POST,
    url: ({ jobId }) => `/v1/jobs/${jobId}/toggle`,
    header: {
      Accept: 'application/json',
      'Content-Type': 'application/json',
    },
    data: ({ enable }) => enable,
  }),
  getTasks: createApiAction<Record<string, Description>, void>({
    method: HttpMethod.GET,
    url: '/v1/jobs/tasks/',
  }),
  fire: createApiAction<void, { jobId: Job['id'] }>({
    method: HttpMethod.POST,
    url: ({ jobId }) => `/v1/jobs/${jobId}/fire`,
  }),

  getState: createApiAction<
    Record<Job['id'], JobTriggerState>,
    { jobIds: Array<Job['id']> }
  >({
    method: HttpMethod.POST,
    url: `/api/v1/jobs/state/`,
    data: ({ jobIds }) => jobIds,
  }),
};
