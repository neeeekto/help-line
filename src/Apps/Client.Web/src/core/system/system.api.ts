import { httpClient, HttpClient } from "@core/http";
import { Message, MessageData, AppState, Settings } from "./system.types";

// eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types
export const makeSystemApi = (http: HttpClient) => ({
  getSettings: () =>
    http.get<Settings>("/api/v1/system/settings").then((x) => x.data),
  getState: () =>
    http.get<AppState>("/api/v1/system/state").then((x) => x.data),
  getMessages: (all = false) =>
    http
      .get<Message[]>(`/api/v1/system/messages?all=${all}`)
      .then((x) => x.data),

  addMessage: (data: MessageData) =>
    http.post<Message>(`/api/v1/system/messages`, data).then((x) => x.data),
  updateMessage: (id: Message["id"], data: MessageData) =>
    http
      .patch<Message>(`/api/v1/system/messages/${id}`, data)
      .then((x) => x.data),
  deleteMessage: (id: Message["id"]) =>
    http.delete(`/api/v1/system/messages/${id}`).then((x) => x.data),
});

export const systemApi = makeSystemApi(httpClient);
