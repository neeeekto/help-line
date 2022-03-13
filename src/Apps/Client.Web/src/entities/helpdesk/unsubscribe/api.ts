import { httpClient, HttpClient } from "@core/http";
import { Unsubscribe } from "./types";

// eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types
export const makeUnsubscribeApi = (http: HttpClient) => ({
  get: () =>
    http.get<Unsubscribe[]>("/api/v1/hd/unsubscribe").then((x) => x.data),
  delete: (unsubscribeId: string) =>
    http
      .delete<void>(`/api/v1/hd/unsubscribe/${unsubscribeId}`)
      .then((x) => x.data),
});

export const unsubscribeApi = makeUnsubscribeApi(httpClient);
