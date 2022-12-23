import { httpClient, HttpClient } from '@core/http';
import { Mock } from './types';

// eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types
export const make_Api = (http: HttpClient) => ({
  get: () => http.get<Mock[]>('/api/v1/hd/__').then(x => x.data),
});

export const _Api = make_Api(httpClient);
