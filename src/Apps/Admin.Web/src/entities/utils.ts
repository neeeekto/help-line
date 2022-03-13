import { HttpClient } from '@core/http';

export const makeCrudApi = <TResult, TData>(http: HttpClient, segment: string) => ({
  get: () => http.get<TResult[]>(`/api/v1/${segment}`).then(x => x.data),
  add: (data: TData) => http.post<void>(`/api/v1/${segment}`, data).then(x => x.data),
  update: (entityId: string, data: TData) => http.patch<void>(`/api/v1/${segment}/${entityId}`, data).then(x => x.data),
  remove: (entityId: string) => http.delete<void>(`/api/v1/${segment}/${entityId}`).then(x => x.data),
});

export const makeCudApi = <TResult, TData>(http: HttpClient, segment: string) => ({
  get: () => http.get<TResult[]>(`/api/v1/${segment}`).then(x => x.data),
  save: (data: TData) => http.post<void>(`/api/v1/${segment}`, data).then(x => x.data),
  remove: (entityId: string) => http.delete<void>(`/api/v1/${segment}/${entityId}`).then(x => x.data),
});
