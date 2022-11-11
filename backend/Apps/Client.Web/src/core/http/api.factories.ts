import { HttpClient } from "@core/http/http.types";

export const makeCrudApi = <TResult, TData>(
  http: HttpClient,
  segment: string
) => ({
  get: () => http.get<TResult[]>(`${segment}`).then((x) => x.data),
  add: (data: TData) => http.post<void>(`${segment}`, data).then((x) => x.data),
  update: (entityId: string, data: TData) =>
    http.patch<void>(`${segment}/${entityId}`, data).then((x) => x.data),
  delete: (entityId: string) =>
    http.delete<void>(`${segment}/${entityId}`).then((x) => x.data),
});

export interface IRudApi<TResult, TData, TKey extends string | number> {
  get: () => Promise<TResult[]>;
  delete: (entityId: TKey) => Promise<void>;
  save: (data: TData, entityId: TKey) => Promise<void>;
}

export const makeRudApi = <
  TResult,
  TData,
  TKey extends string | number = string
>(
  http: HttpClient,
  segment: string
): IRudApi<TResult, TData, TKey> => ({
  get: () => http.get<TResult[]>(`${segment}`).then((x) => x.data),
  save: (data: TData, key: TKey) =>
    http
      .put<void>(`${segment}/${key}`, data, {
        headers: {
          Accept: "application/json",
          "Content-Type": "application/json",
        },
      })
      .then((x) => x.data),
  delete: (entityId: TKey) =>
    http.delete<void>(`${segment}/${entityId}`).then((x) => x.data),
});
