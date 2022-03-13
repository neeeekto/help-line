import { HttpClient, httpClient } from "@core/http";
import { TicketsSettings } from "./types";

export const makeTicketsSettingApi = (http: HttpClient) => ({
  get: () =>
    http.get<TicketsSettings>(`/api/v1/hd/settings`).then((x) => x.data),
  update: (data: TicketsSettings) =>
    http.patch<void>(`/api/v1/hd/settings`, data).then((x) => x.data),
});

export const ticketsSettingApi = makeTicketsSettingApi(httpClient);
