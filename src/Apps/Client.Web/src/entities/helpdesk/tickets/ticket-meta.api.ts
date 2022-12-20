// eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types
import { httpClient, HttpClient } from "@core/http";
import { Description } from "@entities/helpdesk/meta";

export const makeTicketMetaApi = (http: HttpClient) => ({
  getSearchModel: () =>
    http.get<Description>("/api/v1/hd/meta/search-model").then((x) => x.data),
  getCtxModel: () =>
    http.get<Description>("/api/v1/hd/meta/ctx-model").then((x) => x.data),
});

export const ticketMetaApi = makeTicketMetaApi(httpClient);
