import { HttpClient, httpClient, makeCrudApi } from "@core/http";
import { TicketFilter, TicketFilterData } from "./types";

export const makeTicketsFilterApi = (http: HttpClient) => ({
  ...makeCrudApi<TicketFilter, TicketFilterData>(
    http,
    "/api/v1/hd/tickets/search/filters"
  ),
  get: (features?: string[]) =>
    http
      .get<TicketFilter[]>(`/api/v1/hd/tickets/search/filters`, {
        params: { features },
      })
      .then((x) => x.data),
  getById: (filterId: string) =>
    http
      .get<TicketFilter>(`/api/v1/hd/tickets/search/filters/${filterId}`)
      .then((x) => x.data),
});

export const ticketsFilterApi = makeTicketsFilterApi(httpClient);
