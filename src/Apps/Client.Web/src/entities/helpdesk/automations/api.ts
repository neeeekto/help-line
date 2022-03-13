import { httpClient, HttpClient } from "@core/http";
import { AutoReply, AutoReplyData } from "./types";
import { makeRudApi } from "@core/http";

// eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types
export const makeAutomationsApi = <TResult, TData>(
  http: HttpClient,
  segment: string
) => ({
  ...makeRudApi<TResult, TData, string>(
    http,
    `/api/v1/hd/automations/${segment}`
  ),
});

export const autoReplyApi = makeAutomationsApi<AutoReply, AutoReplyData>(
  httpClient,
  "replies"
);
