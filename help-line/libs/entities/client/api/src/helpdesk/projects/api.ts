import { httpClient, HttpClient } from '@core/http';
import { Project } from './types';

// eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types
export const makeProjectApi = (http: HttpClient) => ({
  get: () => http.get<Project[]>(`/api/v1/hd/projects`).then(x => x.data),
});

export const projectApi = makeProjectApi(httpClient);
