/* eslint-disable @typescript-eslint/explicit-module-boundary-types */
import { httpClient, HttpClient } from "@core/http";
import {
  CreateTicketData,
  Ticket,
  TicketAction,
  TicketFilter,
  TicketFilterData,
  TicketsSettings,
} from "./types";
import { Filter, Sort } from "@entities/filter";
import { makeCrudApi } from "@core/http";

export const makeTicketsApi = (http: HttpClient) => ({
  create: (data: CreateTicketData) =>
    http.post<Ticket["id"]>(`/api/v1/hd/tickets`, data).then((x) => x.data),
  search: (page: number, perPage: number, filter: Filter, sorts: Sort[]) =>
    http
      .post<Ticket[]>(
        `/api/v1/hd/tickets/search`,
        {
          filter,
          sorts,
        },
        {
          params: {
            page,
            perPage,
          },
        }
      )
      .then((x) => x.data),
});

export const ticketsApi = makeTicketsApi(httpClient);
