import { httpClient, HttpClient } from "@core/http";
import { MessageTemplate, MessageTemplateData } from "./types";
import { makeCrudApi } from "@core/http";

// eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types
export const makeMessageTemplateApi = (http: HttpClient) => ({
  ...makeCrudApi<MessageTemplate, MessageTemplateData>(
    http,
    `/api/v1/hd/message-templates`
  ),
  changeOrder: (templateId: string, order: number) =>
    http
      .patch<void>(`/api/v1/hd/message-templates/${templateId}/order`, order)
      .then((x) => x.data),
});

export const messageTemplateApi = makeMessageTemplateApi(httpClient);
