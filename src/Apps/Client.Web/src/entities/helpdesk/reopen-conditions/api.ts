import { httpClient, HttpClient } from "@core/http";
import { makeRudApi } from "@core/http";
import {
  ReopenCondition,
  ReopenConditionData,
} from "@entities/helpdesk/reopen-conditions/types";

// eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types
export const makeReopenConditionApi = (http: HttpClient) => ({
  ...makeRudApi<ReopenCondition, ReopenConditionData>(
    http,
    "/api/v1/hd/reopen-conditions"
  ),
  switch: (fromId: string, toId: string) =>
    http
      .post<void>(`/api/v1/hd/reopen-conditions/${fromId}/switch`, toId)
      .then((x) => x.data),
  toggle: (reopenConditionId: string) =>
    http
      .patch<void>(`/api/v1/hd/reopen-conditions/${reopenConditionId}/toggle`)
      .then((x) => x.data),
});

export const reopenConditionApi = makeReopenConditionApi(httpClient);
