/* eslint-disable @typescript-eslint/explicit-module-boundary-types */
import { httpClient, HttpClient } from "@core/http";
import { Ban, BanSettings } from "./types";

export const makeBanApi = (http: HttpClient) => ({
  get: () => http.get<Ban[]>("/api/v1/hd/ban").then((x) => x.data),
  settings: () =>
    http.get<BanSettings>("/api/v1/hd/ban/settings").then((x) => x.data),
  add: (
    parameter: Ban["parameter"],
    value: Ban["value"],
    expiredAt: Ban["expiredAt"]
  ) =>
    http
      .post<void>("/api/v1/hd/ban/settings", { parameter, value, expiredAt })
      .then((x) => x.data),
  delete: (banId: Ban["id"]) =>
    http.delete(`/api/v1/hd/ban/${banId}`).then((x) => x.data),
});

export const banApi = makeBanApi(httpClient);
