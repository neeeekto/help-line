import { httpClient, HttpClient } from "@core/http";
import { SavedReminder, SavedReminderData } from "./types";
import { makeCrudApi } from "@core/http";

// eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types
export const makeRemindersApi = (http: HttpClient) => ({
  ...makeCrudApi<SavedReminder, SavedReminderData>(
    http,
    "/api/v1/hd/reminders"
  ),
  get: (enabled?: boolean) =>
    http
      .get<SavedReminder[]>(`/api/v1/hd/reminders`, { params: { enabled } })
      .then((x) => x.data),
});

export const remindersApi = makeRemindersApi(httpClient);
