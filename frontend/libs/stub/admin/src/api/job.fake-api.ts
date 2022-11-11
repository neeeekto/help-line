import { jobsApi } from '@help-line/api/admin';
import {
  createDeleteStubFactory,
  createFakeApi,
  createGetStubFactory,
  createPathStubFactory,
  createPostStubFactory,
} from '@help-line/stub/share';

export const jobsFakeApi = createFakeApi(jobsApi, {
  get: createGetStubFactory('/api/v1/jobs'),
  getById: createGetStubFactory('/api/v1/jobs/:id'),
  create: createPostStubFactory('/api/v1/jobs/:id'),
  update: createPathStubFactory('/api/v1/jobs/:id'),
  delete: createDeleteStubFactory('/api/v1/jobs/:id'),
  toggle: createPostStubFactory('/api/v1/jobs/toggle/:id'),
  getTasks: createGetStubFactory('/api/v1/jobs/tasks'),
  fire: createPostStubFactory('/api/v1/jobs/fire/:id'),
  getState: createPostStubFactory('/api/v1/jobs/state'),
});
